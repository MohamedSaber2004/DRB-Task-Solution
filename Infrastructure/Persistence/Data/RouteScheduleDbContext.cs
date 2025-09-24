using Microsoft.EntityFrameworkCore;
using System.Reflection;

public class RouteScheduleDbContext(DbContextOptions<RouteScheduleDbContext> options): DbContext(options)
{
    // DbSets For Entities
    public DbSet<Driver> Drivers { get; set; } 
    public DbSet<Route> Routes { get; set; } 
    public DbSet<RouteHistory> RouteHistories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

