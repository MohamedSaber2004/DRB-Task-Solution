using System.Linq.Expressions;

public class AvailableDriversSpecification : BaseSpecifications<Driver, int>
{
    public AvailableDriversSpecification(bool preferAvailableOnly = false)
        : base(BuildCriteria(preferAvailableOnly))
    {
        AddInclude(d => d.Routes);
        AddInclude(d => d.RouteHistories);

        AddOrderByDescending(d => d.Availability);
    }

    private static Expression<Func<Driver, bool>> BuildCriteria(bool preferAvailableOnly)
    {
        var criteria = (Expression<Func<Driver, bool>>)(d => 
            !d.Routes.Any(r => r.RouteStatus == RouteStatus.Assigned || r.RouteStatus == RouteStatus.Active));

        if (preferAvailableOnly)
        {
            return d => d.Availability && 
                       !d.Routes.Any(r => r.RouteStatus == RouteStatus.Assigned || r.RouteStatus == RouteStatus.Active);
        }

        return criteria;
    }
}
