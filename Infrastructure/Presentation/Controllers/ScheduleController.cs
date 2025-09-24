using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController(IServiceManager _serviceManager):ControllerBase
{
    // Schedule Operation
    [HttpPost()]
    public async Task<ActionResult<CreatedResult>> ScheduleOperation([FromBody]ScheduleOperationDto operationDto)
    {
        var result = await _serviceManager.ScheduleService.ScheduleAsync(operationDto);
        return Ok(result);
    }
}

