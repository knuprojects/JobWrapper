using Users.Core.Entities;

namespace Users.Core.Repositories;

public interface IRoleRepository
{
    ValueTask<bool> IsRoleUniqueAsync(string name);

    ValueTask<Role> GetRoleAsync(Guid gid);
    ValueTask<Role> GetRoleByNameAsync(string name);
    ValueTask<IEnumerable<Role>> GetAllRolesAsync();

    void AddRole(Role role);
    void UpdateRole(Role role);
    Guid DeleteRole(Guid gid);
}