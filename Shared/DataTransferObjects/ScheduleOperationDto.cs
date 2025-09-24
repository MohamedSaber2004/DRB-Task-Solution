using System.ComponentModel.DataAnnotations;

public class ScheduleOperationDto
{
    [Required(ErrorMessage = "Operation type is required")]
    [RegularExpression("AssignDriver|ChangeRouteStatus|UnassignDriver|AutoAssignDriver", 
        ErrorMessage = "Operation type must be one of the following: AssignDriver, ChangeRouteStatus, UnassignDriver, AutoAssignDriver")]
    public string OperationType { get; set; } = null!;

    [Required(ErrorMessage = "Route ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Route ID must be greater than 0")]
    public int RouteId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Driver ID must be greater than 0")]
    public int? DriverId { get; set; }

    public RouteStatus? NewStatus { get; set; }
}

