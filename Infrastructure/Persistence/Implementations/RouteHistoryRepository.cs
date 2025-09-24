public class RouteHistoryRepository: GenericRepository<RouteHistory,int>, IRouteHistoryRpository
{
    public RouteHistoryRepository(RouteScheduleDbContext dbContext)
        : base(dbContext)
    {
    }
}

