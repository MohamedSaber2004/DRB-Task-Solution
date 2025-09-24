using AutoMapper;
using Microsoft.Extensions.Logging;

public class ServiceManager(IUnitOfWork _unitOfWork,
                            IMapper _mapper) : IServiceManager
{
    private readonly Lazy<IDriverService> _driverService = new Lazy<IDriverService>(() => new DriverService(_unitOfWork,_mapper));

    private readonly Lazy<IRouteService> _routeService = new Lazy<IRouteService>(() => new RouteService(_unitOfWork,_mapper));

    private readonly Lazy<IScheduleService> _scheduleService = new Lazy<IScheduleService>(() => new ScheduleService(_unitOfWork));

    public IDriverService DriverService => _driverService.Value;

    public IRouteService RouteService => _routeService.Value;

    public IScheduleService ScheduleService => _scheduleService.Value;
}

