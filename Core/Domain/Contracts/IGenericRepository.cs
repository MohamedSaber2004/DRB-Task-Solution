public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
{
    // Get All Entities
    Task<IEnumerable<TEntity>> GetAllAsync();

    // Get Entity By Id
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> spec);

    // Add Entity
    Task AddAsync(TEntity entity);

    // Update Entity
    void Update(TEntity entity);

    // Delete Entity
    void Delete(TEntity entity);

    // Save Changes
    Task<int> SaveChangesAsync();
}

