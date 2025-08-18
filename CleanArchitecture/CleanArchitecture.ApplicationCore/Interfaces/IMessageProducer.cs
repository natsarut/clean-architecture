using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.ApplicationCore.Interfaces
{
    public interface IMessageProducer
    {
        Task PublishAsync<T>(T message, CancellationToken cancellation = default);
        Task SendAsync<T>(T message, string queueName, CancellationToken cancellation = default);
    }
}
