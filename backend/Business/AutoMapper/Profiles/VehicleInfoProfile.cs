using AutoMapper;
using Business.Models.v1.Responses;
using Persistence.Entities.v1;

using static Persistence.Util.Constants;

namespace Business.AutoMapper.Profiles;

public class VehicleInfoProfile : Profile
{
    public VehicleInfoProfile()
    {
        CreateMap<VehicleInfo, VehicleInfoResponse>()
            //Municipality mapping
            .ForMember(x => x.Municipality, opt => opt.MapFrom(x => x.Municipality.Name))
            .ForMember(x => x.MunicipalityMultiplier, opt => opt.MapFrom(x => x.Municipality.Multiplier))
            //Purpose mapping
            .ForMember(x => x.Purpose, opt => opt.MapFrom(x => x.VehiclePurpose.Description))
            .ForMember(x => x.PurposeId, opt => opt.MapFrom(x => x.VehiclePurpose.Id))
            //Tariff type mapping
            .ForMember(x => x.TariffType, opt => opt.MapFrom(x => x.TariffType.Description))
            .ForMember(x => x.TariffTypeId, opt => opt.MapFrom(x => x.TariffTypeId))
            //.ForMember(x => x.EngineVolumeMultiplier, opt => opt.MapFrom(x => x.EngineVolume.Multiplier))
            .ForMember(x => x.EngineVolumeMultiplier, opt 
                => opt.MapFrom(x => x.EngineVolume == null ? DefaultValues.EngineVolumeDefaultMultiplier : x.EngineVolume.Multiplier))
            //Owner age mapping
            .ForMember(x => x.OwnerAge, opt => opt.MapFrom(x => x.OwnerAge))
            //.ForMember(x => x.AgeGroupMultiplier, opt => opt.MapFrom(x => x.AgeGroup.Multiplier))
            .ForMember(x => x.AgeGroupMultiplier, opt 
                => opt.MapFrom(x => x.AgeGroup == null ? DefaultValues.AgeGroupDefaultMultiplier : x.AgeGroup.Multiplier));

    }
}