using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        // Primary Key
        builder.HasKey(r => r.Id);
        
        // Properties
        builder.Property(r => r.StartLocation)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(r => r.EndLocation)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(r => r.Distance)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(r => r.EstimatedTime)
               .IsRequired();

        builder.Property(r => r.RouteStatus)
               .HasConversion<string>()
               .HasMaxLength(50)
               .HasDefaultValue(RouteStatus.Pending);

        // Audit fields
        builder.Property(r => r.CreatedOn)
               .HasDefaultValueSql("GETDATE()");

        builder.Property(r => r.LastModifiedOn)
               .HasComputedColumnSql("GETDATE()");

        // Indexes
        builder.HasIndex(r => r.RouteStatus)
               .HasDatabaseName("IX_Route_RouteStatus");
               
        builder.HasIndex(r => r.DriverId)
               .HasDatabaseName("IX_Route_DriverId");
    }
}

