using Mapster;
using MapsterMapper;

namespace CleanArchitecture.MessageConsumerService.Code
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            // Register mappings here.
            
        }

        public static IMapper CreateMapper()
        {
            // Create a new Mapper instance with the configured mappings.
            return new Mapper(TypeAdapterConfig.GlobalSettings);
        }
    }
}
