namespace CleanArchitecture.WebApi.Code.Options
{
    public record AppConfigOptions
    {
        public const string SectionName = "AppConfig";

        public bool UseInMemoryDatabase { get; init; } = false;
        public string RabbitMqHost { get; init; } = "rabbitmq://localhost";
    }
}
