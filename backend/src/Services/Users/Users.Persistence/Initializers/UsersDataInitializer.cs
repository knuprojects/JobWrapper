using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Dal.Initializers;
using Users.Core.Entities;

namespace Users.Persistence.Initializers;

internal class UsersDataInitializer : IDataInitializer
{
    private readonly UsersContext _dbContext;
    private readonly ILogger<UsersDataInitializer> _logger;

    public UsersDataInitializer(
        UsersContext dbContext,
        ILogger<UsersDataInitializer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task InitAsync()
    {
        if (await _dbContext.Roles.AnyAsync()) return;

        await AddRolesAsync();

        await _dbContext.SaveChangesAsync();
    }

    private async Task AddRolesAsync()
    {
        await _dbContext.Roles.AddAsync(Role.Init(Role.Admin, true));

        await _dbContext.Roles.AddAsync(Role.Init(Role.DefaultRole, true));

        _logger.LogInformation("Initialized roles.");
    }
}