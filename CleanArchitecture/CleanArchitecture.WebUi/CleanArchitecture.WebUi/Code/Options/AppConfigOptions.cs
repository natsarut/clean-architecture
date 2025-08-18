namespace CleanArchitecture.WebUi.Code.Options
{
    public record AppConfigOptions
    {
        public const string SectionName = "AppConfig";

        public required string ApiBaseUrl { get; init; }
    }
}
