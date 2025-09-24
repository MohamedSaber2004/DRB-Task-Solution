public interface IDriverRepository : IGenericRepository<Driver, int>
{
    // Get Driver History
    Task<List<Driver>> GetDriverHistoryAsync(ISpecifications<Driver, int> spec);
    
    // Check If Driver Exists
    Task<bool> ExistsAsync(ISpecifications<Driver,int> spec,string name, string licenseType);
}

