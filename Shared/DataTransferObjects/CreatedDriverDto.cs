using System.ComponentModel.DataAnnotations;

public record CreatedDriverDto(
    [Required(ErrorMessage = "Driver name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Driver name must be between 2 and 100 characters")]
    string Name,

    [Required(ErrorMessage = "License type is required")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "License type must be between 1 and 20 characters")]
    string LicenseType,

    bool Availability = true
);