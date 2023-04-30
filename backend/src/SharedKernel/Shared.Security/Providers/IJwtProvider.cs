using Shared.Abstractions.Time;
using Shared.Security.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shared.Security.Providers;

public interface IJwtProvider
{
    JsonWebToken CreateToken(string userId, string? email, string? role, IDictionary<string, string>? claims);
}

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IUtcClock _utcClock;

    public JwtProvider(
        JwtOptions options,
        IJwtTokenGenerator tokenGenerator,
        IUtcClock utcClock)
    {
        _options = options;
        _tokenGenerator = tokenGenerator;
        _utcClock = utcClock;
    }

    public JsonWebToken CreateToken(string userId, string? email, string? role, IDictionary<string, string>? claims)
    {
        var now = _utcClock.GetCurrentUtcTime();

        var jwtClaims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.Sub, userId),
               new Claim(JwtRegisteredClaimNames.UniqueName, userId),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString())
            };

        if (!string.IsNullOrWhiteSpace(email))
            jwtClaims.Add(new Claim(ClaimTypes.Email, email));

        if (!string.IsNullOrWhiteSpace(role))
            jwtClaims.Add(new Claim(ClaimTypes.Role, role));

        var expires = now.AddMinutes(_options.ExpiryMinutes);

        var jwt = _tokenGenerator.GenerateToken(
            _options.SecretKey,
            _options.Issuer,
            _options.Audience,
            expires,
            jwtClaims);

        return new JsonWebToken
        {
            AccessToken = jwt,
            Expires = expires.ToTimestamp(),
            UserId = userId,
            Role = role ?? string.Empty,
            Claims = jwtClaims
        };
    }
}
