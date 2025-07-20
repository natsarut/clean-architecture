using CleanArchitecture.ApplicationCore.Interfaces;
using CleanArchitecture.ApplicationCore.Services;
using CleanArchitecture.Infrastructure.Databases;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {  
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            // Add infrastructure services to the container.
            services.AddScoped<IUnitOfWork, ApplicationUnitOfWork>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IArtistService, ArtistService>();
        }        
    }
}
