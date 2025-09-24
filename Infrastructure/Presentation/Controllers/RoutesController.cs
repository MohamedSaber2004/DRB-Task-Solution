using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RoutesController(IServiceManager _serviceManager):ControllerBase
{
    // Add Route
    [HttpPost("add")]
    public async Task<ActionResult<CreatedResult>> AddRoute(CreatedRouteDto createdRouteDto)
    {
        var result = await _serviceManager.RouteService.AddRouteAsync(createdRouteDto);
        return CreatedAtAction(nameof(AddRoute), result);
    }

    // Get Routes
    [HttpGet]
    public async Task<ActionResult<PaginatedResult<RouteDto>>> GetRoutes([FromQuery]RouteQueryParams QueryParams)
    {
        var routes = await _serviceManager.RouteService.GetRoutesAsync(QueryParams);
        return Ok(routes);
    }
}
