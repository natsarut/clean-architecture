using CleanArchitecture.MessageConsumerService;
using CleanArchitecture.MessageConsumerService.Extensions;
using CleanArchitecture.MessageConsumerService.Options;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOptions<AppConfigOptions>().Bind(builder.Configuration.GetSection(AppConfigOptions.SectionName));
AppConfigOptions? appConfig = builder.Configuration.GetSection(AppConfigOptions.SectionName).Get<AppConfigOptions>() ?? throw new InvalidOperationException($"Configuration section '{AppConfigOptions.SectionName}' is not found or invalid.");

// Add message bus to the container.
builder.Services.AddMassTransitConsumer(appConfig);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
