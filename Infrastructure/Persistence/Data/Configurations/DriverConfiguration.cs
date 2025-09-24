using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        // Primary Key
        builder.HasKey(d => d.Id);
        
        // Properties
        builder.Property(d => d.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(d => d.LicenseType)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(d => d.Availability)
               .HasDefaultValue(true);

        // Audit fields
        builder.Property(d => d.CreatedOn)
               .HasDefaultValueSql("GETDATE()");

        builder.Property(d => d.LastModifiedOn)
               .HasComputedColumnSql("GETDATE()");

        // Relationships
        builder.HasMany(d => d.Routes)           
               .WithOne(r => r.Driver)
               .HasForeignKey(r => r.DriverId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(d => d.RouteHistories)
               .WithOne(rh => rh.Driver)
               .HasForeignKey(rh => rh.DriverId)
               .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(d => d.Availability)
               .HasDatabaseName("IX_Driver_Availability");
    }
}

