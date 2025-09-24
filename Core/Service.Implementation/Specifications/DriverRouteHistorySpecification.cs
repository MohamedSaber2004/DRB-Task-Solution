public class DriverRouteHistorySpecification: BaseSpecifications<Driver,int>
{
    public DriverRouteHistorySpecification(int id):base(d => d.Id == id)
    {
        AddInclude(d => d.RouteHistories);
        AddInclude(d => d.Routes);
    }
}

