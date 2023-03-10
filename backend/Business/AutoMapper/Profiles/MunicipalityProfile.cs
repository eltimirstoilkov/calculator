using AutoMapper;
using Business.Models.v1.Responses;
using Persistence.Entities.v1;

namespace Business.AutoMapper.Profiles;

public class MunicipalityProfile : Profile
{
    public MunicipalityProfile()
    {
        CreateMap<Municipality, MunicipalityResponse>();
    }
}