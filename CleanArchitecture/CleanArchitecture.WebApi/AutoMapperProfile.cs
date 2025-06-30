using AutoMapper;
using CleanArchitecture.ApplicationCore;
using CleanArchitecture.ApplicationCore.Entities;
using CleanArchitecture.ApplicationCore.Models;
using CleanArchitecture.WebApi.Models;

namespace CleanArchitecture.WebApi
{
    public class AutoMapperProfile: Profile
    {
        private static IMapper? _mapper;

        public AutoMapperProfile()
        {
            CreateMap<Artist, ArtistDto>().ReverseMap();
            CreateMap<ArtistForUpdate, ArtistForUpdateDto>().ReverseMap();
        }

        public static IMapper CreateMapper()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfile());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            return _mapper;
        }
    }
}
