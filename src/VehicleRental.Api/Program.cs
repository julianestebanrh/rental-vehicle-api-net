using Asp.Versioning;
using Asp.Versioning.Builder;
using Serilog;
using VehicleRental.Api.Controllers.Rentals;
using VehicleRental.Api.Extensions;
using VehicleRental.Api.OpenApi;
using VehicleRental.Api.Versioning;
using VehicleRental.Application;
using VehicleRental.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddVersioning();
builder.Services.AddSwagger();

var app = builder.Build();

// Setup MinimalApi

ApiVersionSet apiVersionSet = app.NewApiVersionSet("Rentals")
    .HasApiVersion(new ApiVersion(2))
    .ReportApiVersions()
    .Build();

RouteGroupBuilder versionedGroup = app
    .MapGroup("api/v{apiVersion:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

versionedGroup.MapRentalEndpoints();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });

}

//app.UseHttpsRedirection();

await app.ApplyMigrations();

//app.SeedAuthentication();
//app.SeedVehicles();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;