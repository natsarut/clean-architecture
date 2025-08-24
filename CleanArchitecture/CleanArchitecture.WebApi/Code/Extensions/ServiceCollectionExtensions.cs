using CleanArchitecture.ApplicationCore.Messages;
using CleanArchitecture.WebApi.Code.Options;
using Mapster;
using MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace CleanArchitecture.WebApi.Code.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHealths(this IServiceCollection services,AppConfigOptions appConfig,string connectionString)
        {
            IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

            if (appConfig.UseInMemoryDatabase)
            {
                healthChecksBuilder.AddCheck("InMemory Database", () => HealthCheckResult.Healthy("InMemory database is healthy."), tags: ["Ready"]);
            }
            else
            {
                healthChecksBuilder.AddSqlServer(connectionString, name: "SQL Server", tags: ["Ready"]);
            }

            services.AddHealthChecksUI().AddInMemoryStorage();
        }

        public static void AddObjectMapper(this IServiceCollection services)
        {
            // Register Mapster.
            services.AddMapster();

            // Configure Mapster mappings.
            MapsterConfig.Configure();
        }

        public static void AddWebApiDocuments(this IServiceCollection services)
        {
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();

            // Add Swagger UI to services container.
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public static void AddMassTransitProducer(this IServiceCollection services, AppConfigOptions appConfig)
        {
            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(appConfig.RabbitMqHost,"/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.Message<ArtistCreated>(e => e.SetEntityName(typeof(ArtistCreated).FullName!)); // Name of the primary exchange.
                    cfg.Publish<ArtistCreated>(e => e.ExchangeType = ExchangeType.Direct); // Primary exchange type.
                    cfg.Send<ArtistCreated>(e =>
                    {
                        e.UseRoutingKeyFormatter(context => context.Message.NotificationProvider.ToString()); // Route by notification provider (email or sms).
                    });
                });
            });
        }
    }
}
