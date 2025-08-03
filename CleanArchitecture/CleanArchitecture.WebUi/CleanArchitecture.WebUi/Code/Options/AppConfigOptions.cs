namespace CleanArchitecture.WebUi.Code.Options
{
    public class AppConfigOptions
    {
        public const string SectionName = "AppConfig";

        public required string ApiBaseUrl { get; set; }
    }
}
