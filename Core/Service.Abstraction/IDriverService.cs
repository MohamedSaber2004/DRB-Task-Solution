public interface IDriverService
{
    // Add Driver
    Task<CreatedResult> AddDriverAsync(CreatedDriverDto entity);

    // Get Driver History
    Task<DriverHistoryDto> GetDriverHistoryAsync(int id);
}