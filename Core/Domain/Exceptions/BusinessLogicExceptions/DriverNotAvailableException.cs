public class DriverNotAvailableException(int driverId) : BusinessLogicException($"Driver with Id {driverId} is not available.")
{
}

