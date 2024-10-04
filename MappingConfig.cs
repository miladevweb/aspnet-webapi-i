using AutoMapper;
using webapi_tutorial.Models;
using webapi_tutorial.Models.Dto;

namespace webapi_tutorial;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        // Villa -> VillaDto
        CreateMap<Villa, VillaDto>();
        CreateMap<VillaDto, Villa>();

        CreateMap<Villa, VillaCreateDto>().ReverseMap();
        CreateMap<Villa, VillaUpdateDto>().ReverseMap();

        // NumeroVilla -> NumeroVillaDto
        CreateMap<NumeroVilla, NumeroVillaDto>().ReverseMap();
        CreateMap<NumeroVilla, NumeroVillaCreateDto>().ReverseMap();
        CreateMap<NumeroVilla, NumeroVillaUpdateDto>().ReverseMap();
    }
}

// Then create table in ApplicationDbContext *migrations* folder