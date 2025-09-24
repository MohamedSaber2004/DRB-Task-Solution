public class DuplicateRouteException(string startLocation,string endLocation, double distance, TimeSpan estimatedTime)
    :ConflictException($"route with startLocation '{startLocation}', endLocation '{endLocation}', distance '{distance}', and estimatedTime '{estimatedTime}' already exists in the system.")
{
}

