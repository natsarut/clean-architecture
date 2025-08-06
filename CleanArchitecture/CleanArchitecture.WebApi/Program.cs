using CleanArchitecture.Infrastructure.Databases;
using CleanArchitecture.Infrastructure.Extensions;
using CleanArchitecture.WebApi.Code;
using CleanArchitecture.WebApi.Code.Extensions;
using CleanArchitecture.WebApi.Code.Options;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using System;



Logger? logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    logger.Info("Initializing application...");
    var builder = WebApplication.CreateBuilder(args);
    string? defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(defaultConnectionString))
    {
        throw new InvalidOperationException("DefaultConnection string is not configured.");
    }

    builder.Services.AddControllers(options => 
    { 
        options.ReturnHttpNotAcceptable = true; 
    });

    builder.Services.AddOptions<AppConfigOptions>().Bind(builder.Configuration.GetSection(AppConfigOptions.SectionName)).ValidateDataAnnotations();
    AppConfigOptions? appConfig = builder.Configuration.GetSection(AppConfigOptions.SectionName).Get<AppConfigOptions>() ?? throw new InvalidOperationException($"Configuration section '{AppConfigOptions.SectionName}' is not found or invalid.");
    builder.Services.AddDataProtection();

    // Configure NLog
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    // Configures the Web API document.
    builder.Services.AddWebApiDocuments();

    if (appConfig.UseInMemoryDatabase)
    {
        // Use InMemory database for testing purposes.
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("InMemoryCleanArchitecture"));
        logger.Info("Using InMemory database.");
    }
    else
    {
        // Use SQL Server database.
        // Add connection string to services container.
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(defaultConnectionString));
        logger.Info("Using SQL Server database with connection string: {ConnectionString}", nameof(defaultConnectionString));
    }
    
    // Add infrastructures to the container.
    builder.Services.AddInfrastructureServices();

    // Add object mapper to the container.
    builder.Services.AddObjectMapper();

    // Add health checks to the container.
    builder.Services.AddHealths(appConfig,defaultConnectionString);

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
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception.");
    LogManager.Shutdown(); // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    throw;
}

/// <summary>
/// Used in the integration tests project.
/// </summary>
public partial class Program { }
