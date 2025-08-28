using CleanArchitecture.Infrastructure.Databases;
using CleanArchitecture.Infrastructure.Extensions;
using CleanArchitecture.MessageConsumerService.Code.Extensions;
using CleanArchitecture.MessageConsumerService.Code.Options;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

try
{
    var builder = WebApplication.CreateBuilder(args);
    string? defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(defaultConnectionString))
    {
        throw new InvalidOperationException("DefaultConnection string is not configured.");
    }

    // Add services to the container.
    builder.Services.AddControllers(options =>
    {
        options.ReturnHttpNotAcceptable = true;
    });

    builder.Services.AddOptions<AppConfigOptions>().Bind(builder.Configuration.GetSection(AppConfigOptions.SectionName)).ValidateDataAnnotations();
    AppConfigOptions? appConfig = builder.Configuration.GetSection(AppConfigOptions.SectionName).Get<AppConfigOptions>() ?? throw new InvalidOperationException($"Configuration section '{AppConfigOptions.SectionName}' is not found or invalid.");
    builder.Services.AddDataProtection();

    // Configures the Web API document.
    builder.Services.AddWebApiDocuments();

    if (appConfig.UseInMemoryDatabase)
    {
        // Use InMemory database for testing purposes.
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("InMemoryCleanArchitecture"));
        //logger.Info("Using InMemory database.");
    }
    else
    {
        // Use SQL Server database.
        // Add connection string to services container.
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(defaultConnectionString));
        //logger.Info("Using SQL Server database with connection string: {ConnectionString}", nameof(defaultConnectionString));
    }

    // Add infrastructures to the container.
    builder.Services.AddInfrastructureServices();

    // Add message bus to the container.
    builder.Services.AddMassTransitConsumer(appConfig);

    // Add object mapper to the container.
    builder.Services.AddObjectMapper();

    // Add health checks to the container.
    builder.Services.AddHealths(appConfig, defaultConnectionString);

    var app = builder.Build();
    app.ConfigureExceptionHandler(app.Logger);
    app.Logger.LogInformation("Starting the application.");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    // HealthCheck Middleware
    app.UseHealthChecks("/health", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.MapHealthChecksUI(); // Map Health Checks UI (/healthchecks-ui)
    app.Run();
}
catch (Exception)
{
    throw;
}

/// <summary>
/// Used in the integration tests project.
/// </summary>
public partial class Program { }