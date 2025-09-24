public class RouteSpecification: BaseSpecifications<Route,int>
{
    public RouteSpecification(RouteQueryParams QueryParams)
        :base(R =>
                 (!QueryParams.DriverId.HasValue || R.DriverId == QueryParams.DriverId)
              && (string.IsNullOrWhiteSpace(QueryParams.SearchValue) || 
                  (R.StartLocation.ToLower().Contains(QueryParams.SearchValue.ToLower()) || R.EndLocation.ToLower().Contains(QueryParams.SearchValue.ToLower()))))
    {
        AddInclude(r => r.Driver!);

        switch (QueryParams.SortingOption)
        {
            case RouteSortingOptions.DistanceAsc:
                AddOrderBy(r => r.Distance); 
                break;
            case RouteSortingOptions.DistanceDesc:
                AddOrderByDescending(r => r.Distance);
                break;
        }

        ApplyPaging(QueryParams.Page,QueryParams.PageSize);
    }

    public RouteSpecification(int routeId)
        :base(r => r.Id == routeId)
    {
        AddInclude(r => r.Driver!);
    }
}

