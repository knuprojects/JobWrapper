using Microsoft.EntityFrameworkCore;
using Users.Core.Entities;

namespace Users.Persistence;

public class UsersContext : DbContext
{
    public DbSet<User>? Users { get; set; }
    public DbSet<Role>? Roles { get; set; }

    public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}
