using CleanArchitecture.ApplicationCore.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.MessagingServices
{
    public class EMailConsumer : IConsumer<ArtistCreated>
    {
        public static readonly string QueueName = $"{typeof(ArtistCreated).FullName}.EMail";

        public async Task Consume(ConsumeContext<ArtistCreated> context)
        {
            Console.WriteLine($"E-mail sent for artist was created with ID: {context.Message.ArtistId} to address {context.Message.Target}.");
            await Task.CompletedTask;
        }
    }
}
