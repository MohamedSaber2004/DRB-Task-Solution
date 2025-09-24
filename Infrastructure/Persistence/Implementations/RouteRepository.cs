
using Microsoft.EntityFrameworkCore;

public class RouteRepository : GenericRepository<Route, int>, IRouteRepository
{
    private readonly RouteScheduleDbContext _dbContext;

    public RouteRepository(RouteScheduleDbContext dbContext)
        :base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Route>> GetRoutesAsync(ISpecifications<Route, int> spec) 
        => await SpecificationEvaluator.GetQuery(_dbContext.Routes, spec).ToListAsync();
    public async Task<int> CountAsync(ISpecifications<Route, int> spec) 
        => await SpecificationEvaluator.GetQuery(_dbContext.Routes, spec).CountAsync();

    public async Task<bool> ExistsAsync(ISpecifications<Route, int> spec, params object[] parameters) 
        => await SpecificationEvaluator.GetQuery(_dbContext.Routes.AsNoTracking(), spec).AnyAsync();
}

