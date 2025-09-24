public interface IServiceManager
{
    IDriverService DriverService { get; }

    IRouteService RouteService { get; }

    IScheduleService ScheduleService { get; }
}

