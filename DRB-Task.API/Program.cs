using DRB_Task.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DRB_Task.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5000); 
                options.ListenAnyIP(5001, listenOptions =>
                {
                    listenOptions.UseHttps();
                });
            });

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            #region  Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); 
            
            builder.Services.AddDbContext<RouteScheduleDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetSection("ConnectionStrings")["DatabaseConnection"],
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        
                        sqlOptions.CommandTimeout(60);
                    }));
            
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddAutoMapper(configAction: cfg => { },assemblies: typeof(AssemblyReference).Assembly);
            
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Where(m => m.Value!.Errors.Any())
                                    .Select(e => new ValidationError()
                                    {
                                        Field = e.Key,
                                        Errors = e.Value!.Errors.Select(er => er.ErrorMessage)
                                    });

                    var response = new ValidationErrorToReturn() { Errors = errors };

                    return new BadRequestObjectResult(response);
                };
            });
            #endregion

            var app = builder.Build();

            try
            {
                await app.SeedingDataToDbAsync();
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Database seeding failed during startup. Application will continue without seeding.");
                
                if (app.Environment.IsDevelopment())
                {
                    logger.LogWarning("Seeding failed in development environment. Check your database connection.");
                }
            }

            #region Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseCors();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.ConfigObject = new ConfigObject()
                {
                    DisplayRequestDuration = true
                };

                options.DocumentTitle = "Route Schedule API";

                options.DocExpansion(DocExpansion.None);

                options.EnableFilter();
            });

            app.UseHttpsRedirection();
            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
