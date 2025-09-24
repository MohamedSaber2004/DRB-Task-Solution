using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DriversController(IServiceManager _serviceManager) : ControllerBase
{
    [HttpPost("add")]
    public async Task<ActionResult<CreatedResult>> AddDriver([FromBody] CreatedDriverDto driverDto)
    {

        var result = await _serviceManager.DriverService.AddDriverAsync(driverDto);
        return CreatedAtAction(nameof(AddDriver), result);
    }

    // Get Driver History
    [HttpGet("{id}/history")]
    public async Task<ActionResult<List<DriverHistoryDto>>> GetDriverHistory(int id)
    {
        var history =  await _serviceManager.DriverService.GetDriverHistoryAsync(id);
        return Ok(history);
    }
}