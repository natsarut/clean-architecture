namespace CleanArchitecture.WebUi.Code.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHealths(this IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddHealthChecksUI().AddInMemoryStorage();
        }

        public static void AddWebApiDocuments(this IServiceCollection services)
        {
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();

            // Add Swagger UI to services container.
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
