using AutoMapper;
using ParkyAPIApp.Models;
using ParkyAPIApp.Models.Dtos;

namespace ParkyAPIApp.ParkyMapper
{
    public class ParkyMappings : Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
