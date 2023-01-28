using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.AdditionalAbstractions;
using Shared.Abstractions.Primitives;
using Shared.Abstractions.Serialization;
using Shared.Dal.Transactions;

namespace Shared.Dal.Utils;

public interface IUnitOfWork
{
    Task SaveChangesAsync<TBody>(CancellationToken cancellationToken, params TBody[]? messages) where TBody : class, IEvent;
}

public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{

    private readonly TContext _dbContext;
    private DbSet<OutboxMessage> _set;
    private readonly IJsonSerializer _jsonSerializer;

    public UnitOfWork(
        TContext dbContext,
        IJsonSerializer jsonSerializer)
    {
        _dbContext = dbContext;
        _jsonSerializer = jsonSerializer;
    }

    public async Task SaveChangesAsync<TBody>(CancellationToken cancellationToken, params TBody[]? messages) where TBody : class, IEvent
    {
        ProcessedOutboxMessage(messages);
        UpdateAuditableEntities();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void ProcessedOutboxMessage<TBody>(TBody[]? messages) where TBody : class, IEvent
    {
        if (messages is not EmptyEvent)
        {
            var message = new OutboxMessage
            {
                Gid = Guid.NewGuid(),
                OccuredOnUtc = DateTime.UtcNow,
                Type = messages.GetType().Name,
                Content = _jsonSerializer.Serialize<object>(messages)
            };

            _set.Add(message);
        }
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

            if (entry.State == EntityState.Modified)
                entry.Property(a => a.LastModified)
                    .CurrentValue = DateTime.UtcNow;
        }
    }
}