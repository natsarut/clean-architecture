using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.MessageConsumerService.Options
{
    public record AppConfigOptions
    {
        public const string SectionName = "AppConfig";

        public string RabbitMqHost { get; init; } = "localhost";
    }
}
