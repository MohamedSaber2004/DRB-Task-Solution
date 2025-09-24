public interface IRouteService
{
    Task<CreatedResult> AddRouteAsync(CreatedRouteDto entity);

    Task<PaginatedResult<RouteDto>> GetRoutesAsync(RouteQueryParams QueryParams);
}

