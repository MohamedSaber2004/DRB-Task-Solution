public class RouteExistsSpecification:BaseSpecifications<Route, int>
{
    public RouteExistsSpecification(CreatedRouteDto routeDto)
        :base(r => r.StartLocation == routeDto.StartLocation 
                && r.EndLocation == routeDto.EndLocation 
                && r.Distance == routeDto.Distance 
                && r.EstimatedTime == routeDto.EstimatedTime)
    {
        
    }
}

