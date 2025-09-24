using Microsoft.EntityFrameworkCore;

public class GenericRepository<TEntity, TKey>(RouteScheduleDbContext _dbContext) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbContext.Set<TEntity>().ToListAsync();

    public async Task<TEntity?> GetByIdAsync(TKey id) => await _dbContext.Set<TEntity>().FindAsync(id);

    public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

    public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

    public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();

    public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> spec) 
        => await SpecificationEvaluator.GetQuery(_dbContext.Set<TEntity>(), spec).FirstOrDefaultAsync();
}

