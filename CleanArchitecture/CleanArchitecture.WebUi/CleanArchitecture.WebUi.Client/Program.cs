using CleanArchitecture.WebUi.Client.Code.Interfaces;
using CleanArchitecture.WebUi.Client.Code.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IBffService, BffService>();
await builder.Build().RunAsync();
