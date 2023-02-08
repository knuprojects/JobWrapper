using Microsoft.EntityFrameworkCore;
using Users.Core.Entities;
using Users.Core.Repositories;

namespace Users.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UsersContext _dbContext;

    public UserRepository(UsersContext dbContext)
        => _dbContext = dbContext;

    public async ValueTask<bool?> IsUserNameUniqueAsync(string userName)
    {
        var existingUserName = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == userName);

        if (existingUserName is null) return true;

        return false;
    }

    public async ValueTask<bool?> IsEmailUniqueAsync(string? email)
    {
        var existingEmail = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);

        if (existingEmail is null) return true;

        return false;
    }

    public async ValueTask<IEnumerable<User?>> GetAllUsersAsync() => await _dbContext.Users.ToListAsync();

    public async ValueTask<User?> GetByEmailAsync(string email) => await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);

    public async ValueTask<User?> GetByUserNameAsync(string userName) => await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == userName);

    public async ValueTask<User?> GetUserAsync(Guid gid) => await _dbContext.Users.SingleOrDefaultAsync(x => x.Gid == gid);

    public void AddUser(User user) => _dbContext.Users.Add(user);

    public void UpdateUser(User user) => _dbContext.Users.Update(user);

    public Guid DeleteUser(Guid gid)
    {
        var existingUser = _dbContext.Users.SingleOrDefault(x => x.Gid == gid);

        if (existingUser is not null)
        {
            _dbContext.Users.Remove(existingUser);

            return existingUser.Gid;
        }

        return Guid.Empty;
    }
}