using Microsoft.EntityFrameworkCore;

public class ScheduleRepository : GenericRepository<Driver, int>, IScheduleRepository
{
    private readonly RouteScheduleDbContext _dbContext;

    public ScheduleRepository(RouteScheduleDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<bool> HasActiveRoute(ISpecifications<Driver, int> spec)
        => await SpecificationEvaluator.GetQuery(_dbContext.Drivers.AsNoTracking(), spec).AnyAsync();

    public async Task<Driver?> GetFirstAvailableDriverAsync(ISpecifications<Driver, int> spec) 
        => await SpecificationEvaluator.GetQuery(_dbContext.Drivers.AsNoTracking(), spec).FirstOrDefaultAsync();
}

