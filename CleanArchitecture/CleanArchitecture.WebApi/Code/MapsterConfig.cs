using CleanArchitecture.ApplicationCore.Entities;
using CleanArchitecture.ApplicationCore.Models;
using CleanArchitecture.WebApi.Models;
using Mapster;
using MapsterMapper;

namespace CleanArchitecture.WebApi.Code
{
    public static class MapsterConfig
    {
        public static void Configure()
        {             
            // Register mappings here.
            TypeAdapterConfig<Artist, ArtistDto>.NewConfig();
            TypeAdapterConfig<ArtistForUpdate, ArtistForUpdateDto>.NewConfig();
        }

        public static IMapper CreateMapper()
        {
            // Create a new Mapper instance with the configured mappings.
            return new Mapper(TypeAdapterConfig.GlobalSettings);
        }
    }
}
