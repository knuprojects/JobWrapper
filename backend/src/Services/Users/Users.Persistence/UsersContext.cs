using Microsoft.EntityFrameworkCore;
using Shared.Dal.Outbox;
using Users.Core.Entities;

namespace Users.Persistence;

public class UsersContext : DbContext
{
    public DbSet<User>? Users { get; set; }
    public DbSet<Role>? Roles { get; set; }
    public DbSet<OutboxMessage> Outbox { get; set; } = null!;

    public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}
