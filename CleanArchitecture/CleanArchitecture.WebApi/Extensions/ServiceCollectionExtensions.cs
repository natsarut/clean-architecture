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

        public static void AddApplicationCoreServices(this IServiceCollection services)
        {
            // Add application core services to the container.
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IArtistService, ArtistService>();
        }

        public static void AddInfrastructures(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork,ApplicationUnitOfWork>();
        }
    }
}
