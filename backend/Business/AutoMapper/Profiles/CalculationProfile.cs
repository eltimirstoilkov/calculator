using AutoMapper;
using Business.Models.v1.Responses;
using Persistence.Entities.v1;

namespace Business.AutoMapper.Profiles
{
    public class CalculationProfile : Profile
    {
        public CalculationProfile()
        {
            CreateMap<Calculation, CalculationResponse>();
        }
    }
}
