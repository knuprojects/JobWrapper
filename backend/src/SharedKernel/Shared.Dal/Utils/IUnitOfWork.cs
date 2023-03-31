using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Primitives;

namespace Shared.Dal.Utils;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _dbContext;

    public UnitOfWork(
        TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        UpdateAuditableEntities();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities()
    {
        var entries = _dbContext.ChangeTracker
            .Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Property(a => a.CreatedAt)
                    .CurrentValue = DateTime.UtcNow;

            if (entry.State == EntityState.Modified || entry.State == EntityState.Detached)
                entry.Property(a => a.LastModified)
                    .CurrentValue = DateTime.UtcNow;
        }
    }
}