public sealed class DriverExistsSpecification:BaseSpecifications<Driver,int>
{
    public DriverExistsSpecification(string name,string licenseType)
        :base(d => d.Name == name && d.LicenseType == licenseType)
    {
    }
}
