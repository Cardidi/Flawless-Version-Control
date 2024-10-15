using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Flawless.Server.Utility;

public static class AuthUtility
{
    private static JwtSecurityTokenHandler _tokenHandler = new();

    private static SymmetricSecurityKey? _key;

    public static string GenerateSecret(
        string randomRange = "abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()_+=-", 
        int length = 256 / 8)
    {
        var rng = Random.Shared;
        
        String ran = "";
        for (int i = 0; i < length; i++) 
        { 
            int x = rng.Next(randomRange.Length); 
            ran += randomRange[x]; 
        }

        return ran;
    }
    
    public static string JwtSecret { get; private set; } = GenerateSecret();
    
    public static string Issuer { get; private set; } = Environment.GetEnvironmentVariable("issuer") ?? "jwt";
    
    public static string Audience { get; private set; } = Environment.GetEnvironmentVariable("audience") ?? "jwt";

    public static SymmetricSecurityKey SecurityKey
    {
        get
        {
            if (_key == null) _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
            return _key;
        }
    }

    public static void ResetKey(string issuer, string audience, string? keySecret = null)
    {
        JwtSecret = keySecret ?? GenerateSecret();
        Issuer = issuer;
        Audience = audience;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
    }
    
    public static string GenerateJwtToken(string username, uint expires)
    {
        var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature); 
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, username),
        };

        var token = _tokenHandler.CreateJwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            subject: new ClaimsIdentity(claims),
            expires: DateTime.Now.AddSeconds(expires),
            issuedAt: DateTime.Now,
            notBefore: DateTime.Now,
            signingCredentials: credentials);
        
        return _tokenHandler.WriteToken(token);
    }
}