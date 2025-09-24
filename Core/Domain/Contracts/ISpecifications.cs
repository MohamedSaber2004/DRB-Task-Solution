using System.Linq.Expressions;

public interface ISpecifications<TEntity,TKey> where TEntity : BaseEntity<TKey>
{
    Expression<Func<TEntity,bool>>? Criteria { get; }
    Expression<Func<TEntity,object>> OrderBy { get; }
    Expression<Func<TEntity,object>> OrderByDescending { get; }
    List<Expression<Func<TEntity,object>>> Includes { get; }

    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}

