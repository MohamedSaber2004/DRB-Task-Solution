using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DataSeeding(RouteScheduleDbContext _dbContext) : IDataSeeding
{
    public async Task DataSeedAsync()
    {
        try
        {
            // Check If Found Any PendingMigrations
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
                await _dbContext.Database.MigrateAsync();

            // Seeding To Drivers
            if (!await _dbContext.Drivers.AnyAsync())
            {
                var driversJsonData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\drivers.json");
                var driversData = await JsonSerializer.DeserializeAsync<List<Driver>>(driversJsonData);
                if (driversData is not null && driversData.Any())
                {
                    await _dbContext.Drivers.AddRangeAsync(driversData);
                    await _dbContext.SaveChangesAsync();
                }
            }

            // Configure JSON options for enum handling
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            // Seeding To Routes
            if (!await _dbContext.Routes.AnyAsync())
            {
                var routesJsonData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\routes.json");
                var routesData = await JsonSerializer.DeserializeAsync<List<Route>>(routesJsonData,jsonOptions);
                if (routesData is not null && routesData.Any())
                {
                    await _dbContext.Routes.AddRangeAsync(routesData);
                    await _dbContext.SaveChangesAsync();
                }
            }

            // Seeding To RouteHistories
            if (!await _dbContext.RouteHistories.AnyAsync())
            {
                var routeHistoriesJsonData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\route-histories.json");
                var routeHistoriesData = await JsonSerializer.DeserializeAsync<List<RouteHistory>>(routeHistoriesJsonData);
                if (routeHistoriesData is not null && routeHistoriesData.Any())
                {
                    await _dbContext.RouteHistories.AddRangeAsync(routeHistoriesData);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

