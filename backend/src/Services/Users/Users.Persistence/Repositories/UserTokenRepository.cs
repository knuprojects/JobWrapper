using Microsoft.EntityFrameworkCore;
using Users.Core.Entities;
using Users.Core.Repositories;

namespace Users.Persistence.Repositories;

public sealed class UserTokenRepository : IUserTokenRepository
{
    private readonly UsersContext _dbContext;

    public UserTokenRepository(UsersContext dbContext)
        => _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async ValueTask<bool?> CheckUserTokenAsync(string token)
    {
        var existingToken = await _dbContext.UserTokens.SingleOrDefaultAsync(x => x.ConfirmationToken == token);

        bool? isTokenExpired = UserToken.IsExpired(existingToken, DateTimeOffset.UtcNow);

        if (isTokenExpired.GetValueOrDefault())
            return true;

        return false;
    }
}