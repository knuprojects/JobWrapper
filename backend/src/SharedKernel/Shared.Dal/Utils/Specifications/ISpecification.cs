using System.Linq.Expressions;

namespace Shared.Dal.Utils.Specifications;

public interface ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>> Criteria { get; }
    List<Expression<Func<TEntity, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
}

public class BaseSpecification<TEntity> : ISpecification<TEntity>
{
    protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected BaseSpecification()
    {
    }

    public Expression<Func<TEntity, bool>> Criteria { get; protected set; }
    public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
    public List<string> IncludeStrings { get; } = new List<string>();
}