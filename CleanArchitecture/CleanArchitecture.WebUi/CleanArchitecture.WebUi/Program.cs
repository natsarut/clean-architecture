using CleanArchitecture.WebUi.Code.Extensions;
using CleanArchitecture.WebUi.Code.Services;
using CleanArchitecture.WebUi.Components;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NLog;
using NLog.Web;

Logger? logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllers(options =>
    {
        options.ReturnHttpNotAcceptable = true;
    });

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddInteractiveWebAssemblyComponents();
    builder.Services.AddDataProtection();

    // Configure NLog
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    // Configures the Web API document.
    builder.Services.AddWebApiDocuments();

    builder.Services.AddAntiforgery(options =>
    {
        options.HeaderName = "X-CSRF-TOKEN";
        options.SuppressXFrameOptionsHeader = true; // Optional: if you want to allow framing
    });

    // Add health checks to the container.
    builder.Services.AddHealths();

    builder.Services.AddHttpClient<ArtistService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7049");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAntiforgery();
    app.UseAuthorization();
    app.MapStaticAssets();
    app.MapControllers();
    app.MapControllerRoute(name: "default",pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();
    app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(CleanArchitecture.WebUi.Client._Imports).Assembly);

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
    logger.Error(ex, "Application start-up failed.");
    LogManager.Shutdown(); // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    throw;
}   

