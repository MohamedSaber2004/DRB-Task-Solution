
public class UnitOfWork(RouteScheduleDbContext _dbContext) : IUnitOfWork
{
    private readonly Lazy<IDriverRepository> _driverRepository = new Lazy<IDriverRepository>(() => new DriverRepository(_dbContext));
    private readonly Lazy<IRouteRepository> _routeRepository = new Lazy<IRouteRepository>(() => new RouteRepository(_dbContext));
    private readonly Lazy<IScheduleRepository> _scheduleRepository = new Lazy<IScheduleRepository>(() => new ScheduleRepository(_dbContext));
    private readonly Lazy<IRouteHistoryRpository> _routeHistoryRepository = new Lazy<IRouteHistoryRpository>(() => new RouteHistoryRepository(_dbContext));

    public IDriverRepository Drivers => _driverRepository.Value;

    public IRouteRepository Routes => _routeRepository.Value;

    public IScheduleRepository Schedules => _scheduleRepository.Value;

    public IRouteHistoryRpository RouteHistories => _routeHistoryRepository.Value;

    public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
}

