using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Abstractions.Primitives;
using Shared.Dal.Outbox;
using Shared.Messaging;

namespace Shared.Dal.Utils;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken, IMessage message);
}

public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _dbContext;
    private DbSet<OutboxMessage> _outbox;
    public UnitOfWork(
        TContext dbContext)
    {
        _dbContext = dbContext;
        _outbox = dbContext.Set<OutboxMessage>();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken, IMessage message)
    {
        UpdateAuditableEntities();
        OutboxMessageProcessing(message);

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

    private void OutboxMessageProcessing(IMessage message)
    {
        if (message is EmptyMessage)
            return;

        var outboxMessage = new OutboxMessage
        {
            Type = message.GetType().ToString(),
            OccuredAt = DateTimeOffset.UtcNow,
            Content = JsonConvert.SerializeObject(
                message,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
        };

        _outbox.Add(outboxMessage);
    }
}