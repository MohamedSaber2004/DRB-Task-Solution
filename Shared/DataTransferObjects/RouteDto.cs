public record RouteDto(
    string StartLocation,
    string EndLocation,
    double Distance,
    TimeSpan EstimatedTime,
    string RouteStatus,
    string DriverName
);

