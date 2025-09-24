using AutoMapper;

public class DriverService(IUnitOfWork _unitOfWork, IMapper _mapper) : IDriverService       
{
    public async Task<CreatedResult> AddDriverAsync(CreatedDriverDto entity)
    {        
        var spec = new DriverExistsSpecification(entity.Name, entity.LicenseType);
        var driverExists = await _unitOfWork.Drivers.ExistsAsync(spec,entity.Name, entity.LicenseType);
        if (driverExists)
            throw new DuplicateDriverException(entity.Name, entity.LicenseType);
       
        var driver = _mapper.Map<Driver>(entity);
        await _unitOfWork.Drivers.AddAsync(driver);
        await _unitOfWork.SaveChangesAsync();
        return new CreatedResult { Message = "Driver is added successfully." };
    }

    public async Task<DriverHistoryDto> GetDriverHistoryAsync(int id)
    {
        var driver = await _unitOfWork.Drivers.GetByIdAsync(id);
        if (driver is null)
            throw new DriverNotFoundException(id);

        var spec = new DriverRouteHistorySpecification(id);
        var driversWithHistory = await _unitOfWork.Drivers.GetDriverHistoryAsync(spec);

        var driverWithHistory = driversWithHistory.FirstOrDefault();
        if (driverWithHistory is null)
        {
            return new DriverHistoryDto
            {
                Name = driver.Name,
                History = []
            };
        }

        return _mapper.Map<DriverHistoryDto>(driverWithHistory);
    }
}