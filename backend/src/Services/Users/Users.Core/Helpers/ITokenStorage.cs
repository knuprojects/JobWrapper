using Microsoft.AspNetCore.Http;
using Users.Core.Dto;

namespace Users.Core.Helpers;

public interface ITokenStorage
{
    void Set(JwtDto jwt);
    JwtDto? Get();
}

public class HttpTokenStorage : ITokenStorage
{
    private const string TokenKey = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpTokenStorage(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    public void Set(JwtDto jwt) => _httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwt);

    public JwtDto? Get()
    {
        if (_httpContextAccessor.HttpContext is null) return null;

        if (_httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var jwt)) return jwt as JwtDto;

        return null;
    }
}