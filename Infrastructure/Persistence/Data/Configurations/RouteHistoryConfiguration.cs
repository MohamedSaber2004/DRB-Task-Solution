using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RouteHistoryConfiguration : IEntityTypeConfiguration<RouteHistory>
{
    public void Configure(EntityTypeBuilder<RouteHistory> builder)
    {
        // Primary Key
        builder.HasKey(rh => rh.Id);
        
        // Properties
        builder.Property(rh => rh.StartLocation)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(rh => rh.EndLocation)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(rh => rh.CompletedOn)
               .IsRequired();

        // Foreign Key Properties
        builder.Property(rh => rh.DriverId)
               .IsRequired();

        builder.Property(rh => rh.RouteId)
               .IsRequired(false);

        // Audit fields
        builder.Property(rh => rh.CreatedOn)
               .HasDefaultValueSql("GETDATE()");

        builder.Property(rh => rh.LastModifiedOn)
               .HasComputedColumnSql("GETDATE()"); 

        // Relationships        
        builder.HasOne(rh => rh.Route)
               .WithMany() 
               .HasForeignKey(rh => rh.RouteId)
               .OnDelete(DeleteBehavior.SetNull)
               .IsRequired(false);

        // Indexes
        builder.HasIndex(rh => rh.DriverId)
               .HasDatabaseName("IX_RouteHistory_DriverId");

        builder.HasIndex(rh => rh.RouteId)
               .HasDatabaseName("IX_RouteHistory_RouteId");
    }
}