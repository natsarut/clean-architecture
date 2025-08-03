namespace CleanArchitecture.WebApi.Code.Options
{
    public class AppConfigOptions
    {
        public const string SectionName = "AppConfig";

        public bool UseInMemoryDatabase { get; set; } = false;
    }
}
