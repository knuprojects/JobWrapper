using Shared.Abstractions.Primitives;

namespace Users.Core.Entities;

public sealed class UserToken : Entity
{
    private UserToken(
        string confirmationToken,
        Guid userGid) : base()
    {
        ConfirmationToken = confirmationToken;
        UserGid = userGid;
        Expired = DateTimeOffset.UtcNow.AddDays(1);
    }

    public string ConfirmationToken { get; private set; } = default!;
    public DateTimeOffset Expired { get; private set; }
    public bool? IsProcessed { get; private set; }
    public Guid UserGid { get; private set; }
    public User? User { get; set; }

    public static UserToken Init(
        string token,
        Guid userGid)
    {
        return new UserToken(token, userGid);
    }

    public static UserToken ProcessToken(UserToken userToken)
    {
        userToken.IsProcessed = true;

        return userToken;
    }

    public static bool? IsExpired(
        UserToken userToken,
        DateTimeOffset date)
    {
        if (userToken.Expired < date)
            return true;

        return false;
    }

    public static UserToken ClearToken(UserToken userToken)
    {
        userToken.IsProcessed = false;

        return userToken;
    }
}