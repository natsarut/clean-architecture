using CleanArchitecture.ApplicationCore.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.MessagingServices
{
    public class MessageProducer(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider) : IMessageProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        private readonly ISendEndpointProvider _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));

        public async Task PublishAsync<T>(T message, CancellationToken cancellation = default)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }
                
            await _publishEndpoint.Publish(message, cancellation);
        }

        public async Task SendAsync<T>(T message, string queueName, CancellationToken cancellation = default)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            ISendEndpoint endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));
            await endpoint.Send(message, cancellation);
        }
    }
}
