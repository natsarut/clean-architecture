using CleanArchitecture.ApplicationCore;
using CleanArchitecture.ApplicationCore.Messages;
using CleanArchitecture.Infrastructure.MessagingServices;
using CleanArchitecture.MessageConsumerService.Options;
using MassTransit;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.MessageConsumerService.Extensions
{
    public static class ServiceCollectionExtensions
    {
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
