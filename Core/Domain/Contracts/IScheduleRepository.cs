public interface IScheduleRepository:IGenericRepository<Driver,int>
{
    Task<bool> HasActiveRoute(ISpecifications<Driver,int> spec);

    Task<Driver?> GetFirstAvailableDriverAsync(ISpecifications<Driver,int> spec);
}

