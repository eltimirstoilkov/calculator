using AutoMapper;
using Business.AutoMapper.Profiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Business.AutoMapper;

public static class ConfigureAutoMapperServices
{
    public static IMapper ConfigureAutomapper(this IServiceCollection services)
    {
        var config = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile(new VehicleInfoProfile());
            configuration.AddProfile(new PurposeProfile());
            configuration.AddProfile(new TariffTypeProfile());
            configuration.AddProfile(new MunicipalityProfile());
            configuration.AddProfile(new CalculationProfile());
            configuration.AddProfile(new AgeGroupProfile());
            configuration.AddProfile(new VehicleTypeProfile());
        });

        var mapper = config.CreateMapper();
        services.TryAddSingleton(mapper);
        return mapper;
    }
}