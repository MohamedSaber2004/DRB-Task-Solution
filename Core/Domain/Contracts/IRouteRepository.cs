public interface IRouteRepository: IGenericRepository<Route,int>
{
    Task<List<Route>> GetRoutesAsync(ISpecifications<Route,int> spec);

    Task<int> CountAsync(ISpecifications<Route,int> spec);

    Task<bool> ExistsAsync(ISpecifications<Route,int> spec,params object[] parameters);
}

