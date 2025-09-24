public class RouteAlreadyAssignedException(int routeId, int driverId)
    : BusinessLogicException($"Route with Id {routeId} is already assigned to Driver with Id {driverId}.")
{
}

