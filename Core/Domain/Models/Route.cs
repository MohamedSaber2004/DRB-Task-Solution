public class Route: BaseEntity<int>
{
    // Properties
    public string StartLocation { get; set; } = null!;
    public string EndLocation { get; set; } = null!;
    public double Distance { get; set; }
    public TimeSpan EstimatedTime { get; set; }
    public RouteStatus RouteStatus { get; set; } = RouteStatus.Pending;

    // Navigation Properties
    public Driver? Driver { get; set; }
    public int? DriverId { get; set; } // FK
}

