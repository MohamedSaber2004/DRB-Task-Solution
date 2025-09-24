public static class ApplicationServicesRegisteration
{
    public static async Task<IApplicationBuilder> SeedingDataToDbAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
        await dataSeeding.DataSeedAsync();
        return app;
    }
}

