using Microsoft.EntityFrameworkCore;
using Users.Core.Entities;
using Users.Core.Repositories;

namespace Users.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly UsersContext _dbContext;

    public RoleRepository(UsersContext dbContext)
        => _dbContext = dbContext;

    public async ValueTask<bool> IsRoleUniqueAsync(string name)
    {
        var existingName = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Name == name);

        if (existingName is null) return true;

        return false;
    }

    public async ValueTask<IEnumerable<Role>> GetAllRolesAsync() => await _dbContext.Roles.ToListAsync();

    public async ValueTask<Role> GetRoleAsync(Guid gid) => await _dbContext.Roles.SingleOrDefaultAsync(x => x.Gid == gid);

    public async ValueTask<Role> GetRoleByNameAsync(string name) => await _dbContext.Roles.SingleOrDefaultAsync(x => x.Name == name);

    public void AddRole(Role role) => _dbContext.Roles.Add(role);

    public void UpdateRole(Role role) => _dbContext.Roles.Update(role);

    public Guid DeleteRole(Guid gid)
    {
        var existingRole = _dbContext.Roles.SingleOrDefault(x => x.Gid == gid);

        if (existingRole is not null)
        {
            _dbContext.Roles.Remove(existingRole);

            return existingRole.Gid;
        }

        return Guid.Empty;
    }
}