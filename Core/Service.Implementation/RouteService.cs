using AutoMapper;

public class RouteService(IUnitOfWork _unitOfWork,
                          IMapper _mapper) : IRouteService
{
    public async Task<CreatedResult> AddRouteAsync(CreatedRouteDto entity)
    {
        var spec = new RouteExistsSpecification(entity);
        var routeExists = await _unitOfWork.Routes.ExistsAsync(spec,entity.StartLocation,entity.EndLocation,entity.Distance,entity.EstimatedTime);
        if(routeExists)
            throw new DuplicateRouteException(entity.StartLocation, entity.EndLocation,entity.Distance,entity.EstimatedTime);

        var route = _mapper.Map<Route>(entity);
        await _unitOfWork.Routes.AddAsync(route);
        await _unitOfWork.SaveChangesAsync();
        return new CreatedResult() { Message = "Route is added successfully." };
    }

    public async Task<PaginatedResult<RouteDto>> GetRoutesAsync(RouteQueryParams QueryParams)
    {
        var spec = new RouteSpecification(QueryParams);
        var routes = await _unitOfWork.Routes.GetRoutesAsync(spec);
        var routesDto = _mapper.Map<IEnumerable<Route>, IEnumerable<RouteDto>>(routes);
        var routesCount = routes.Count();
        var countSpec = new RouteCountSpecfication(QueryParams);
        var totalCount = await _unitOfWork.Routes.CountAsync(countSpec);
        return new PaginatedResult<RouteDto>(QueryParams.Page,QueryParams.PageSize,totalCount,routesDto.ToList());
    }
}

