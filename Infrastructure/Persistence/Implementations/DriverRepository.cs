using Microsoft.EntityFrameworkCore;

public class DriverRepository : GenericRepository<Driver, int>, IDriverRepository
{
    private readonly RouteScheduleDbContext _dbContext;

    public DriverRepository(RouteScheduleDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Driver>> GetDriverHistoryAsync(ISpecifications<Driver, int> spec) 
        => await SpecificationEvaluator.GetQuery(_dbContext.Drivers.AsNoTracking(), spec).ToListAsync();

    public async Task<bool> ExistsAsync(ISpecifications<Driver, int> spec, string name, string licenseType) 
        => await SpecificationEvaluator.GetQuery(_dbContext.Drivers.AsNoTracking(), spec).AnyAsync();

    public async Task<List<Driver>> GetRouteHistoriesAsync(ISpecifications<Driver, int> spec, int driverId) 
        => await SpecificationEvaluator.GetQuery(_dbContext.Drivers.AsNoTracking(), spec).ToListAsync();
}

