public class DuplicateDriverException : ConflictException
{
    public DuplicateDriverException(string name, string licenseType) 
        : base($"A driver with name '{name}' and license type '{licenseType}' already exists in the system.")
    {
        DriverName = name;
        LicenseType = licenseType;
    }

    public string DriverName { get; }
    public string LicenseType { get; }
}