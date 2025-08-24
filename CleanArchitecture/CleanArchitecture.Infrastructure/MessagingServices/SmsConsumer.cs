using CleanArchitecture.ApplicationCore.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.MessagingServices
{
    public class SmsConsumer : IConsumer<ArtistCreated>
    {
        public static readonly string QueueName = $"{typeof(ArtistCreated).FullName}.Sms";

        public async Task Consume(ConsumeContext<ArtistCreated> context)
        {
            Console.WriteLine($"SMS sent for artist was created with ID: {context.Message.ArtistId} to number {context.Message.Target}.");
            await Task.CompletedTask;
        }
    }
}
