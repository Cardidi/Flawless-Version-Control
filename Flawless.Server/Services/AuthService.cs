using Flawless.Api;
using Flawless.Server.Utility;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Flawless.Server.Services;

public class AuthService : Auth.AuthBase
{
    
    private ILogger<AuthService> _logger;

    public AuthService(ILogger<AuthService> logger)
    {
        _logger = logger;
    }
    
    public override Task<AuthResult> GainToken(AuthRequest request, ServerCallContext context)
    {

        if (request.UserName != "admin")
        {
            return Task.FromResult(new AuthResult()
            {
                Token = "",
                Result = -1,
                Message = "Invalid username or password"
            });
        }
        
        var token = AuthUtility.GenerateJwtToken(request.UserName, request.Expires);
        
        _logger.LogInformation($"User '{request.UserName}' has been login in.'");
        return Task.FromResult(new AuthResult
        {
            Token = token,
            Result = 0
        });
    }

    [Authorize]
    public override Task<AuthUserInfo> GetUserInfo(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new AuthUserInfo
        {
            UserName = context.GetHttpContext().User.Identity?.Name ?? string.Empty,
            IsSystemAdmin = true,
            UserId = 1000
        });
    }

    [Authorize]
    public override Task<Empty> Validate(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new Empty());
    }
}