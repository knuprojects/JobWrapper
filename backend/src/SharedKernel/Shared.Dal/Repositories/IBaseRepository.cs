using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Primitives;
using Shared.Dal.Pagination;
using System.Linq.Expressions;

namespace Shared.Dal.Repositories;

public interface IBaseRepository
{
    ValueTask<bool?> IsFieldUniqueAsync<TEntity>(Expression<Func<TEntity, bool>> condition) where TEntity : Entity;
    ValueTask<TEntity> GetByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> condition) where TEntity : Entity;
    ValueTask<PaginationResponse<TEntity>> GetListAsync<TEntity>(PaginationFilter filter) where TEntity : Entity;
    ValueTask<PaginationResponse<TEntity>> GetEntitiesByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> condition, PaginationFilter filter) where TEntity : Entity;
    void Add<TEntity>(TEntity entity) where TEntity : Entity;
    void AddMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity;
    void Update<TEntity>(TEntity entity) where TEntity : Entity;
    Guid Delete<TEntity>(Guid gid) where TEntity : Entity;
}

public class BaseRepository<TContext> : IBaseRepository where TContext : DbContext
{
    private readonly TContext _context;

    public BaseRepository(TContext context)
        => _context = context ?? throw new ArgumentNullException(nameof(context));

    public async ValueTask<bool?> IsFieldUniqueAsync<TEntity>(Expression<Func<TEntity, bool>> condition) where TEntity : Entity
    {
        var existingName = await _context.Set<TEntity>().Where(condition).SingleOrDefaultAsync();

        if (existingName is null) return true;

        return false;
    }
    public async ValueTask<PaginationResponse<TEntity>> GetListAsync<TEntity>(PaginationFilter filter) where TEntity : Entity
        => await _context.Set<TEntity>().AsNoTracking().PaginateAsync(filter);

    public async ValueTask<PaginationResponse<TEntity>> GetEntitiesByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> condition, PaginationFilter filter) where TEntity : Entity
        => await _context.Set<TEntity>().AsNoTracking().Where(condition).PaginateAsync(filter);

    public async ValueTask<TEntity> GetByConditionAsync<TEntity>(Expression<Func<TEntity, bool>> condition) where TEntity : Entity
        => await _context.Set<TEntity>().AsNoTracking().Where(condition).SingleOrDefaultAsync();

    public void AddMany<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        => _context.Set<TEntity>().AddRange(entities);

    public void Add<TEntity>(TEntity entity) where TEntity : Entity
        => _context.Set<TEntity>().Add(entity);

    public void Update<TEntity>(TEntity entity) where TEntity : Entity
        => _context.Set<TEntity>().Update(entity);

    public Guid Delete<TEntity>(Guid gid) where TEntity : Entity
    {
        var existingEntity = _context.Set<TEntity>().SingleOrDefault(x => x.Gid == gid);

        if (existingEntity is not null)
        {
            _context.Set<TEntity>().Remove(existingEntity);

            return existingEntity.Gid;
        }

        return Guid.Empty;
    }
}