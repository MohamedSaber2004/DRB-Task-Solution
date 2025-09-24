public class Driver: BaseEntity<int>
{
    // Properties
    public string Name { get; set; } = null!;
    public string LicenseType { get; set; } = null!;
    public bool Availability { get; set; } = true;

    // Navigation Properties
    public ICollection<Route> Routes { get; set; } = new List<Route>();
    public ICollection<RouteHistory> RouteHistories { get; set; } = new List<RouteHistory>();
}

