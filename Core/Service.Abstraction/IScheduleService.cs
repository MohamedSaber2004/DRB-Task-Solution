public interface IScheduleService
{
    Task<CreatedResult> ScheduleAsync(ScheduleOperationDto operationDto);
}

