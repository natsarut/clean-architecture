using CleanArchitecture.ApplicationCore;
using CleanArchitecture.ApplicationCore.Messages;
using CleanArchitecture.Infrastructure.MessagingServices;
using CleanArchitecture.MessageConsumerService.Code.Options;
using Mapster;
using MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace CleanArchitecture.MessageConsumerService.Code.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHealths(this IServiceCollection services, AppConfigOptions appConfig, string connectionString)
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

        public static void AddMassTransitConsumer(this IServiceCollection services, AppConfigOptions appConfig)
        {
            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(appConfig.RabbitMqHost, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    // Configure the consumer for ArtistCreated messages.
                    cfg.ReceiveEndpoint(EMailConsumer.QueueName, re =>
                    {
                        // Turns off default fanout settings.
                        re.ConfigureConsumeTopology = false;

                        // A replicated queue to provide high availability and data safety. Available in RMQ 3.8+.
                        re.SetQuorumQueue();

                        // Enables a lazy queue for more stable cluster with better predictive performance.
                        // Please note that you should disable lazy queues if you require really high performance, if the queues are always short, or if you have set a max-length policy.
                        re.SetQueueArgument("declare", "lazy");

                        re.Consumer<EMailConsumer>();
                        re.Bind(typeof(ArtistCreated).FullName, e =>
                        {
                            e.RoutingKey = NotificationProviders.Email.ToString();
                            e.ExchangeType = ExchangeType.Direct;
                        });
                    });

                    cfg.ReceiveEndpoint(SmsConsumer.QueueName, re =>
                    {
                        // Turns off default fanout settings.
                        re.ConfigureConsumeTopology = false;

                        // A replicated queue to provide high availability and data safety. Available in RMQ 3.8+.
                        re.SetQuorumQueue();

                        // Enables a lazy queue for more stable cluster with better predictive performance.
                        // Please note that you should disable lazy queues if you require really high performance, if the queues are always short, or if you have set a max-length policy.
                        re.SetQueueArgument("declare", "lazy");

                        re.Consumer<SmsConsumer>();
                        re.Bind(typeof(ArtistCreated).FullName, e =>
                        {
                            e.RoutingKey = NotificationProviders.Sms.ToString();
                            e.ExchangeType = ExchangeType.Direct;
                        });
                    });
                });
            });
        }
    }
}
