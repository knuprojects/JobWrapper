using Users.Core.Entities;

namespace Users.Core.Repositories;

public interface IUserRepository
{
    ValueTask<bool?> IsEmailUniqueAsync(string? email);
    ValueTask<bool?> IsUserNameUniqueAsync(string? userName);

    ValueTask<User?> GetUserAsync(Guid gid);
    ValueTask<User?> GetByUserNameAsync(string? userName);
    ValueTask<User?> GetByEmailAsync(string? email);
    ValueTask<IEnumerable<User?>> GetAllUsersAsync();

    void AddUser(User user);
    void UpdateUser(User user);
    Guid DeleteUser(Guid gid);
}