using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Primitives;
using Shared.Dal.Utils.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dal.Repositories;

public interface IEntityRepository<TEntity>
    where TEntity : Entity
{
    Task<TEntity> GetByIdAsync(Guid gid);
    Task<TEntity> GetBySpecAsync(ISpecification<TEntity> spec);
    Task<TEntity> GetBySpecAsync(Expression<Func<TEntity, bool>> criteria);
    ValueTask<List<TEntity>> ListAsync(string key);
    ValueTask<List<TEntity>> ListAsync(ISpecification<TEntity> spec);
    ValueTask<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> criteria);
    ValueTask AddAsync(string key, TEntity entity);
    ValueTask UpdateAsync(string key, TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    ValueTask UpdateRangeAsync(List<TEntity> entities);
    ValueTask UpdateRangeAsync(IQueryable<TEntity> entities);
    ValueTask DeleteByIdAsync(Guid gid);

    TEntity GetById(Guid gid);
    TEntity GetBySpec(ISpecification<TEntity> spec);
    IQueryable<TEntity> ListAll(string key);
    IQueryable<TEntity> List(ISpecification<TEntity> spec);
    IQueryable<TEntity> List(Expression<Func<TEntity, bool>> criteria);
    void Add(string key, TEntity entity);
    void Update(string key, TEntity entity);
    void Delete(string key, Guid gid);
}

public class EntityRepository<TEntity> : IDisposable, IEntityRepository<TEntity>
    where TEntity : Entity
{
    private DbContext _dbContext;
    //private readonly ICacheService _cacheService;
    private bool _disposed;

    public EntityRepository(
        DbContext dbContext)
        //ICacheService cacheService)
    {
        _dbContext = dbContext;
        //_cacheService = cacheService;
    }

    public async Task<TEntity> GetByIdAsync(Guid gid) => await _dbContext.Set<TEntity>().SingleOrDefaultAsync(x => x.Gid == gid);

    public async Task<TEntity> GetBySpecAsync(ISpecification<TEntity> spec)
    {
        try
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_dbContext.Set<TEntity>().AsQueryable(), (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));

            return await secondaryResult.Where(spec.Criteria).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<TEntity> GetBySpecAsync(Expression<Func<TEntity, bool>> criteria)
    {
        try
        {
            return await _dbContext.Set<TEntity>().Where(criteria).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async ValueTask<List<TEntity>> ListAsync(string key)
    {
        //if (!string.IsNullOrWhiteSpace(key))
        //{
        //    var result = //_cacheService.GetData<List<TEntity>>(key);

        //    if (result is not null)
        //        return result;
        //}

        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public async ValueTask<List<TEntity>> ListAsync(ISpecification<TEntity> spec)
    {
        var queryableResultWithIncludes = spec.Includes
                .Aggregate(_dbContext.Set<TEntity>().AsQueryable(), (current, include) => current.Include(include));

        var secondaryResult = spec.IncludeStrings
            .Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));

        return await secondaryResult.Where(spec.Criteria).ToListAsync();
    }

    public async ValueTask<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> criteria) => await _dbContext.Set<TEntity>().Where(criteria).ToListAsync();

    public async ValueTask AddAsync(string key, TEntity entity)
    {
        var storedData = await _dbContext.Set<TEntity>().AddAsync(entity);

        await _dbContext.SaveChangesAsync();

        //_cacheService.SetData(key, storedData, DateTime.Now.AddMinutes(2));
    }

    public async ValueTask UpdateAsync(string key, TEntity entity)
    {
        var storedData = _dbContext.Set<TEntity>().Update(entity);

        await _dbContext.SaveChangesAsync();

        //_cacheService.SetData(key, storedData, DateTime.Now.AddMinutes(2));
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().AddRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask UpdateRangeAsync(List<TEntity> entities)
    {
        foreach (var entity in entities)
            _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask UpdateRangeAsync(IQueryable<TEntity> entities)
    {
        foreach (var entity in entities)
            _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask DeleteByIdAsync(Guid gid)
    {
        var existingData = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Gid == gid);

        _dbContext.Set<TEntity>().Remove(existingData);

        await _dbContext.SaveChangesAsync();
    }

    public TEntity GetById(Guid gid) => _dbContext.Set<TEntity>().SingleOrDefault(x => x.Gid == gid);

    public TEntity GetBySpec(ISpecification<TEntity> spec)
    {
        var queryableResultWithIncludes = spec.Includes
               .Aggregate(_dbContext.Set<TEntity>().AsQueryable(), (current, include) => current.Include(include));

        var secondaryResult = spec.IncludeStrings
            .Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));

        return secondaryResult.Where(spec.Criteria).FirstOrDefault();
    }

    public IQueryable<TEntity> ListAll(string key)
    {
        //if (!string.IsNullOrWhiteSpace(key))
        //{
        //    //var result = _cacheService.GetData<IQueryable<TEntity>>(key);

        //    if (result is not null)
        //        return result;
        //}

        return _dbContext.Set<TEntity>().AsQueryable();
    }

    public IQueryable<TEntity> List(ISpecification<TEntity> spec)
    {
        var queryableResultWithIncludes = spec.Includes
                .Aggregate(_dbContext.Set<TEntity>().AsQueryable(), (current, include) => current.Include(include));

        var secondaryResult = spec.IncludeStrings
            .Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));

        return secondaryResult.Where(spec.Criteria).AsQueryable();
    }

    public IQueryable<TEntity> List(Expression<Func<TEntity, bool>> criteria)
    {
        try
        {
            return _dbContext.Set<TEntity>().Where(criteria).AsQueryable();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void Add(string key, TEntity entity)
    {
        var storedData = _dbContext.Set<TEntity>().Add(entity);

        //_cacheService.SetData(key, storedData, DateTime.Now.AddMinutes(2));
    }

    public void Update(string key, TEntity entity)
    {
        var storedData = _dbContext.Set<TEntity>().Add(entity);

        //_cacheService.SetData(key, storedData, DateTime.Now.AddMinutes(2));
    }

    public void Delete(string key, Guid gid)
    {
        var existingData = _dbContext.Set<TEntity>().FirstOrDefault(x => x.Gid == gid);

        _dbContext.Set<TEntity>().Remove(existingData);

        //_cacheService.RemoveData(key);
    }

    ~EntityRepository()
    {
        Dispose(true);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        _ = disposing;

        if (!_disposed)
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }

        _disposed = true;
    }

}