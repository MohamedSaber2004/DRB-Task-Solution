using Microsoft.Extensions.Logging;

public class ScheduleService(IUnitOfWork _unitOfWork) : IScheduleService
{
    public Task<CreatedResult> ScheduleAsync(ScheduleOperationDto operationDto)
    {
        return operationDto.OperationType switch
        {
            "AssignDriver" => AssignDriverToRouteAsync(operationDto),
            "ChangeRouteStatus" => ChangeRouteStatusAsync(operationDto),
            "UnassignDriver" => UnassignDriverFromRouteAsync(operationDto),
            "AutoAssignDriver" => AutoAssignDriverToRouteAsync(operationDto),
            _ => throw new UnSupportedException("Unsupported schedule operation type."),
        };
    }

    private async Task<CreatedResult> AssignDriverToRouteAsync(ScheduleOperationDto operation)
    {
        if(!operation.DriverId.HasValue)
            throw new BusinessLogicException("Driver Id must be provided for assigning a driver to a route.");

        var routeId = operation.RouteId;
        var driverId = operation.DriverId.Value;

        var route = await _unitOfWork.Routes.GetByIdAsync(routeId)
                       ?? throw new RouteNotFoundException(routeId);

        var driver = await _unitOfWork.Drivers.GetByIdAsync(driverId)
                       ?? throw new DriverNotFoundException(driverId);

        if(!driver.Availability)
            throw new DriverNotAvailableException(driverId);

        if (await HasActiveRoute(driverId))
            throw new BusinessLogicException($"Driver '{driver.Name}' already has an active route assigned and cannot handle multiple routes.");

        if (route.DriverId.HasValue)
            throw new RouteAlreadyAssignedException(routeId, route.DriverId.Value);

        route.DriverId = driverId;
        route.RouteStatus = RouteStatus.Assigned;
        route.LastModifiedOn = DateTime.Now;

        await CreateScheduleAuditEntryAsync(route, driverId, "AssignDriver", $"Driver '{driver.Name}' assigned to route");

        _unitOfWork.Routes.Update(route);
        await _unitOfWork.SaveChangesAsync();

        return new CreatedResult() { Message = $"Driver '{driver.Name}' has been successfully assigned to route from '{route.StartLocation}' to '{route.EndLocation}'." };
    }

    private async Task<CreatedResult> AutoAssignDriverToRouteAsync(ScheduleOperationDto operation)
    {
        var routeId = operation.RouteId;

        var spec = new RouteSpecification(routeId);
        var route = await _unitOfWork.Routes.GetByIdAsync(spec)
                       ?? throw new RouteNotFoundException(routeId);

        if (route.DriverId.HasValue)
            throw new RouteAlreadyAssignedException(routeId, route.DriverId.Value);

        var availableDriver = await FindBestAvailableDriver();

        if (availableDriver is null)
        {
            route.RouteStatus = RouteStatus.Unassigned;
            route.DriverId = null;

            _unitOfWork.Routes.Update(route);
            await _unitOfWork.SaveChangesAsync();

            return new CreatedResult() { Message = $"No available drivers found. Route from '{route.StartLocation}' to '{route.EndLocation}' has been marked as unassigned." };
        }

        route.DriverId = availableDriver.Id;
        route.RouteStatus = RouteStatus.Assigned;
        route.LastModifiedOn = DateTime.Now;

        await CreateScheduleAuditEntryAsync(route, availableDriver.Id, "AutoAssignDriver", $"Driver '{availableDriver.Name}' automatically assigned");

        _unitOfWork.Routes.Update(route);
        await _unitOfWork.SaveChangesAsync();

        return new CreatedResult() { Message = $"Driver '{availableDriver.Name}' has been automatically assigned to route from '{route.StartLocation}' to '{route.EndLocation}'." };
    }

    private async Task<CreatedResult> ChangeRouteStatusAsync(ScheduleOperationDto operation)
    {
        if (!operation.NewStatus.HasValue)
            throw new BusinessLogicException("New status is required for change status operation.");

        var routeId = operation.RouteId;
        var newStatus = operation.NewStatus.Value;

        var route = await _unitOfWork.Routes.GetByIdAsync(routeId)
                              ?? throw new RouteNotFoundException(routeId);

        ValidateStatusChange(route.RouteStatus, newStatus, route.DriverId);

        var oldStatus = route.RouteStatus;
        route.RouteStatus = newStatus;
        route.LastModifiedOn = DateTime.Now;

        if (newStatus == RouteStatus.Unassigned)
        {
            route.DriverId = null;
        }

        if (route.DriverId.HasValue)
        {
            await CreateScheduleAuditEntryAsync(route, route.DriverId.Value, "ChangeRouteStatus", $"Status changed from '{oldStatus}' to '{newStatus}'");
        }

        if (newStatus == RouteStatus.Completed && route.DriverId.HasValue)
        {
            await CreateRouteHistoryEntryAsync(route);
        }

        _unitOfWork.Routes.Update(route);
        await _unitOfWork.SaveChangesAsync();

        return new CreatedResult() { Message = $"Route status has been changed from '{oldStatus}' to '{newStatus}'." };
    }

    private async Task<CreatedResult> UnassignDriverFromRouteAsync(ScheduleOperationDto operation)
    {
        var routeId = operation.RouteId;
        
        var spec = new RouteSpecification(routeId);
        var route = await _unitOfWork.Routes.GetByIdAsync(spec)
                       ?? throw new RouteNotFoundException(routeId);

        if (!route.DriverId.HasValue)
            throw new BusinessLogicException($"Route with id = {routeId} has no driver assigned.");

        var driverName = route.Driver?.Name ?? "Unknown Driver";
        var driverId = route.DriverId.Value;

        await CreateScheduleAuditEntryAsync(route, driverId, "UnassignDriver", $"Driver '{driverName}' unassigned from route");

        route.DriverId = null;
        route.RouteStatus = RouteStatus.Unassigned;
        route.LastModifiedOn = DateTime.Now;

        _unitOfWork.Routes.Update(route);
        await _unitOfWork.SaveChangesAsync();

        return new CreatedResult { Message = $"Driver '{driverName}' has been successfully unassigned from route from '{route.StartLocation}' to '{route.EndLocation}'. Route is now available for assignment." };
    }

    private async Task CreateScheduleAuditEntryAsync(Route route, int driverId, string operationType, string description)
    {
        var scheduleAudit = new RouteHistory
        {
            StartLocation = route.StartLocation,
            EndLocation = route.EndLocation,
            CompletedOn = DateTime.Now,
            DriverId = driverId,
            RouteId = route.Id,
            OperationType = operationType,
            Description = description
        };

        await _unitOfWork.RouteHistories.AddAsync(scheduleAudit);
    }

    private async Task CreateRouteHistoryEntryAsync(Route route)
    {
        var routeHistory = new RouteHistory
        {
            StartLocation = route.StartLocation,
            EndLocation = route.EndLocation,
            CompletedOn = DateTime.Now,
            DriverId = route.DriverId!.Value,
            RouteId = route.Id,
            OperationType = "RouteCompleted",
            Description = "Route completed successfully"
        };

        await _unitOfWork.RouteHistories.AddAsync(routeHistory);
    }

    private async Task<bool> HasActiveRoute(int driverId)
    {
        var spec = new ScheduleHasActiveRouteSpecification(driverId);
        return await _unitOfWork.Schedules.HasActiveRoute(spec);
    }

    private async Task<Driver?> FindBestAvailableDriver()
    {
        var spec = new AvailableDriversSpecification(preferAvailableOnly: true);
        return await _unitOfWork.Schedules.GetFirstAvailableDriverAsync(spec);
    }

    private static void ValidateStatusChange(RouteStatus currentStatus, RouteStatus newStatus, int? driverId)
    {
        switch (newStatus)
        {
            case RouteStatus.Assigned:
                if (!driverId.HasValue)
                    throw new BusinessLogicException("Cannot set route status to 'Assigned' without a driver.");
                break;

            case RouteStatus.Active:
                if (currentStatus != RouteStatus.Assigned)
                    throw new BusinessLogicException("Route must be 'Assigned' before it can be set to 'Active'.");
                if (!driverId.HasValue)
                    throw new BusinessLogicException("Cannot set route status to 'Active' without an assigned driver.");
                break;

            case RouteStatus.Completed:
                if (currentStatus != RouteStatus.Active)
                    throw new BusinessLogicException("Route must be 'Active' before it can be set to 'Completed'.");
                break;

            case RouteStatus.Pending:
                break;

            case RouteStatus.Unassigned:
                break;

            default:
                throw new BusinessLogicException($"Invalid route status: {newStatus}");
        }
    }
}

