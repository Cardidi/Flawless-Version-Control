using Flawless.Server.Services;
using Flawless.Server.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

bool init = true;
while (init)
{
    init = false;
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddGrpc(x =>
    {
    });

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            RequireExpirationTime = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthUtility.SecurityKey,
            ValidIssuer = AuthUtility.Issuer,
            ValidAudience = AuthUtility.Audience,
            ClockSkew = TimeSpan.Zero,
        };
    });

    builder.Services.AddAuthorization();

    using var app = builder.Build();

    // Enable call router
    app.UseRouting();

    // Enable authentication
    app.UseAuthentication();
    app.UseAuthorization();

    // Configure the HTTP request pipeline.
    app.MapGrpcService<AuthService>();
    app.MapGet("/",
        () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

    app.Run();
}
