using System.Linq.Expressions;

//
public abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    // Constructors
    protected BaseSpecifications(Expression<Func<TEntity, bool>>? criteria)
    {
        Criteria = criteria;
        OrderBy = null!;
        OrderByDescending = null!;
    }

    // Properties
    public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

    public Expression<Func<TEntity, object>> OrderBy { get; private set; }

    public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnabled { get; set; }

    // Methods
    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) => Includes.Add(includeExpression);

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) => OrderBy = orderByExpression;

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression) => OrderByDescending = orderByDescendingExpression;

    protected void ApplyPaging(int Page, int PageSize)
    {
        IsPagingEnabled = true;
        Take = PageSize;
        Skip = (Page - 1) * PageSize;
    }
}

