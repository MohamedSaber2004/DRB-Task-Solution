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
                options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DatabaseConnection"]));
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddAutoMapper(configAction: cfg => { },assemblies: typeof(AssemblyReference).Assembly);
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

            await app.SeedingDataToDbAsync();

            #region Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
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
            }

            app.UseHttpsRedirection();
            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
