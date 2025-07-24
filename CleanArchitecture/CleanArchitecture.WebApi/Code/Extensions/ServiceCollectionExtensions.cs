namespace CleanArchitecture.WebApi.Code.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHealths(this IServiceCollection services,string connectionString)
        {
            services.AddHealthChecks().AddSqlServer(connectionString);
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
