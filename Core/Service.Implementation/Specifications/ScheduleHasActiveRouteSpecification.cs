public class ScheduleHasActiveRouteSpecification: BaseSpecifications<Driver,int>
{
    public ScheduleHasActiveRouteSpecification(int? driverId)
        :base(d => 
                  (!driverId.HasValue || d.Id == driverId)
                && d.Routes.Any(r => r.RouteStatus == RouteStatus.Assigned || r.RouteStatus == RouteStatus.Active))
    {
        AddInclude(d => d.Routes);
    }
}

