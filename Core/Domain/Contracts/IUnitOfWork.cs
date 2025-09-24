public interface IUnitOfWork
{
    IDriverRepository Drivers { get; }

    IRouteRepository Routes { get; }

    IScheduleRepository Schedules { get; }

    IRouteHistoryRpository RouteHistories { get; }

    Task<int> SaveChangesAsync();
}

