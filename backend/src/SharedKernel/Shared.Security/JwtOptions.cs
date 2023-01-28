namespace Shared.Security;

public sealed class JwtOptions
{
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string ValidAudience { get; set; } = null!;
    public int ExpiryMinutes { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateIssuer { get; set; }
}
