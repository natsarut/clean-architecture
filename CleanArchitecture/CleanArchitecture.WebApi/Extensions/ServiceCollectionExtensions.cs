using CleanArchitecture.ApplicationCore.Interfaces;
using CleanArchitecture.ApplicationCore.Services;
using CleanArchitecture.Infrastructure.Databases;

namespace CleanArchitecture.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
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
