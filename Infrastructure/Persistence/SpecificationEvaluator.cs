using Microsoft.EntityFrameworkCore;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> spec) where TEntity : BaseEntity<TKey>
    {
        var query = inputQuery;

        // Add Criteria Expression
        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        // Apply OrderBy Expression
        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);
        else if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);

        // Add Include Expressions
        if (spec.Includes is not null && spec.Includes.Count > 0)
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        // Apply paging if enabled
        if (spec.IsPagingEnabled)
            query = query.Skip(spec.Skip).Take(spec.Take);

        return query;
    }
}

