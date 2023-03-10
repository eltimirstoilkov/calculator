using Business.AutoMapper;
using Business.Implementations.v1;
using Business.Interfaces.v1;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.v1;
using Persistence.Implementations.v1;
using Persistence.Interfaces.v1;

namespace Vehicles.Web;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();
        Configure(app);

        app.Run();

    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VehicleContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        
        services.AddSingleton(configuration);
        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        services.ConfigureAutomapper();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //register repositories 
        services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
        services.AddScoped<IPurposeRepository, PurposeRepository>();
        services.AddScoped<IVehicleInfoRepository, VehicleInfoRepository>();
        services.AddScoped<ITariffTypeRepository, TariffTypeRepository>();
        services.AddScoped<IEngineVolumeRepository, EngineVolumeRepository>();
        services.AddScoped<IAgeGroupRepository, AgeGroupRepository>();
        services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
        services.AddScoped<ICalculationRepository, CalculationRepository>();

        //register services
        services.AddScoped<ICalculationService, CalculationService>();
        services.AddScoped<IPurposeService, PurposeService>();
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IMunicipalityService, MunicipalityService>();
        services.AddScoped<ITariffTypeService, TariffTypeService>();
        services.AddScoped<IAgeGroupService, AgeGroupService>();
        services.AddScoped<IVehicleTypeService, VehicleTypeService>();

    }

    private static void Configure(WebApplication app)
    {
        // Configure the HTTP request pipeline.tariffs
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }

        app.UseCors(options =>
            options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
        );

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}