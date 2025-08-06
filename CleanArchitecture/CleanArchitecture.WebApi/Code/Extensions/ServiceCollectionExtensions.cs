using CleanArchitecture.WebApi.Code.Options;
using Mapster;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.WebApi.Code.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHealths(this IServiceCollection services,AppConfigOptions appConfig,string connectionString)
        {
            IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

            if (appConfig.UseInMemoryDatabase)
            {
                healthChecksBuilder.AddCheck("InMemory Database", () => HealthCheckResult.Healthy("InMemory database is healthy."), tags: ["Ready"]);
            }
            else
            {
                healthChecksBuilder.AddSqlServer(connectionString, name: "SQL Server", tags: ["Ready"]);
            }

            services.AddHealthChecksUI().AddInMemoryStorage();
        }

        public static void AddObjectMapper(this IServiceCollection services)
        {
            // Register Mapster.
            services.AddMapster();

            // Configure Mapster mappings.
            MapsterConfig.Configure();
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
