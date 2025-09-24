using System.ComponentModel.DataAnnotations;

public class CreatedRouteDto
{
    [Required(ErrorMessage = "Start location is required")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Start location must be between 2 and 200 characters")]
    public string StartLocation { get; set; } = null!;

    [Required(ErrorMessage = "End location is required")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "End location must be between 2 and 200 characters")]
    public string EndLocation { get; set; } = null!;

    [Required(ErrorMessage = "Distance is required")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Distance must be greater than 0")]
    public double Distance { get; set; }

    [Required(ErrorMessage = "Estimated time is required")]
    public TimeSpan EstimatedTime { get; set; }
}

