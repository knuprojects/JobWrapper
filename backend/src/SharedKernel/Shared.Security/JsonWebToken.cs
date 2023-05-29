using System.Security.Claims;

namespace Shared.Security;

public sealed class JsonWebToken
{
    public string AccessToken { get; set; } = null!;
    public long Expires { get; set; }
    public string UserId { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Email { get; set; } = null!;
    public IEnumerable<Claim>? Claims { get; set; }
}