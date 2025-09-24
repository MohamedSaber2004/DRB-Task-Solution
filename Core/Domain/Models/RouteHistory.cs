public class RouteHistory : BaseEntity<int>
{
    // Properties
    public string StartLocation { get; set; } = null!;
    public string EndLocation { get; set; } = null!;
    public DateTime CompletedOn { get; set; }
    public string? OperationType { get; set; } 
    public string? Description { get; set; }

    // Navigation Property
    public Driver Driver { get; set; } = null!;
    public int DriverId { get; set; } // FK

    public Route? Route { get; set; }
    public int? RouteId { get; set; } // FK
}