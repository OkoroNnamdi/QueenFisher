
using AutoMapper;
using QueenFisher.Core.DTO;


namespace QueenFisher.Core.Utilities
{
    public class MapInitializer : Profile
    {
        public Mapper regMapper { get; set; }
        public MapInitializer()
        {
            //var regConfig = new MapperConfiguration(conf => conf.CreateMap<RegisterDTO, AppUser>());
            //regMapper = new Mapper(regConfig);

        }
    }
}
