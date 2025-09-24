public class RouteCountSpecfication:BaseSpecifications<Route,int>
{
    public RouteCountSpecfication(RouteQueryParams QueryParams)
          : base(R =>
                 (!QueryParams.DriverId.HasValue || R.DriverId == QueryParams.DriverId)
              && (string.IsNullOrWhiteSpace(QueryParams.SearchValue) ||
                  (R.StartLocation.ToLower().Contains(QueryParams.SearchValue.ToLower()) || R.EndLocation.ToLower().Contains(QueryParams.SearchValue.ToLower()))))
    {
        
    }
}
