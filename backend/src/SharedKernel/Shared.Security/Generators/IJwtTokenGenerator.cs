using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shared.Security.Generators;

public interface IJwtTokenGenerator
{
    string GenerateToken(string secretKey, string issuer, string audience, DateTime utcExpirationTime,
        IEnumerable<Claim> claims);
}

public class JwtTokenGenerator : IJwtTokenGenerator
{
    public string GenerateToken(string secretKey, string issuer, string audience, DateTime utcExpirationTime,
        IEnumerable<Claim> claims)
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            DateTime.UtcNow,
            utcExpirationTime,
            credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
