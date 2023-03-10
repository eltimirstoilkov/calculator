using AutoMapper;
using Business.AutoMapper.Profiles;
using Business.Exceptions;
using Business.Implementations.v1;
using Business.Models.v1.Requests;
using Business.Models.v1.Responses;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Xunit;

namespace Tests.Services;

public class CalculationServiceTests
{
    [Theory]
    [InlineData(2, 3, 3, 2, 1, 1432.08)]
    [InlineData(2, 5, 3, 2, 1, 2028.78)]
    [InlineData(2, 5, 10, 2, 1, 1352.52)]
    public async Task CalculateAsync_GivenValidInput_ShouldReturnValidCalculation(
        int vehicleTariffTypeId, int municipalityId, int vehiclePurposeId, int vehicleTypeId, int OwnerAgeId, decimal finalPremium)
    {
        Mock<IVehicleInfoRepository> vehicleRepository = new();
        vehicleRepository.Setup(x => x.GetTotalPaidAmountAsync())
            .ReturnsAsync(200_00M);
        vehicleRepository.Setup(x => x.GetTotalPendingAmountAsync())
            .ReturnsAsync(100_000M);
        vehicleRepository.Setup(x => x.GetPolicyCountAsync())
            .ReturnsAsync(200);

        Mock<ITariffTypeRepository> tariffTypeRepository = new();
        tariffTypeRepository.Setup(x => x.GetByIdAsync(vehicleTariffTypeId))
            .ReturnsAsync(new VehicleTariffType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IVehicleTypeRepository> vehicleTypeRepository = new();
        vehicleTypeRepository.Setup(x => x.GetByIdAsync(vehicleTypeId))
            .ReturnsAsync(new VehicleType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IEngineVolumeRepository> engineVolumeRepository = new();
        engineVolumeRepository.Setup(x => x.GetByIdAsync(vehicleTariffTypeId))
            .ReturnsAsync(new EngineVolume
            {
                Id = 2,
                Description = "Mock description",
                Multiplier = 1.02M
            });

        Mock<IPurposeRepository> purposeRepository = new();
        purposeRepository.Setup(x => x.GetByIdAsync(vehiclePurposeId))
            .ReturnsAsync(new VehiclePurpose
            {
                Id = 3,
                Description = "Mock description"
            });

        Mock<IAgeGroupRepository> ageGroupRepository = new();
        ageGroupRepository.Setup(x => x.GetByIdAsync(OwnerAgeId))
            .ReturnsAsync(new AgeGroup
            {
                Id = 1,
                Description = "Test description",
                Multiplier = 1.5m
            });

        Mock<IMunicipalityRepository> municipalityRepository = new();
        List<Municipality> municipalities = new List<Municipality>()
        {
            new Municipality
            {
                Id = 3,
                Name = "TestName",
                Multiplier = 1.2m
            },

            new Municipality
            {
                Id = 5,
                Name = "TestName",
                Multiplier = 1.7m
            },

            new Municipality
            {
                Id = 10,
                Name = "TestName",
                Multiplier = 1.4m
            }
        };
        municipalityRepository.Setup(x => x.GetByIdAsync(municipalityId))
            .ReturnsAsync(() => municipalities.FirstOrDefault(x => x.Id == municipalityId));


        Mock<ICalculationRepository> calculationRepository = new();
        Mock<ILogger<CalculationService>> logger = new();

        CalculationService calculationService = new CalculationService(
            vehicleRepository.Object,
            tariffTypeRepository.Object,
            engineVolumeRepository.Object,
            ageGroupRepository.Object,
            municipalityRepository.Object,
            calculationRepository.Object,
            purposeRepository.Object,
            logger.Object,
            CreateMapper()
        );

        var calculationRequest = new CalculationRequest
        {
            VehicleTariffTypeId = vehicleTariffTypeId,
            MunicipalityId = municipalityId,
            VehiclePurposeId = vehiclePurposeId,
            VehicleTypeId = vehicleTypeId,
            OwnerAgeId = OwnerAgeId,
        };

        CalculationResponse calculation = await calculationService.CalculateAsync(calculationRequest);

        decimal expectedBasePremium = 780.0m;
        decimal expectedFinalPremium = finalPremium;

        Assert.Equal(expectedBasePremium, calculation.BasePremium);
        Assert.Equal(expectedFinalPremium, calculation.FinalPremium);
    }


    [Theory]
    [InlineData(2, 4, 3, 2, 1, 3.5, 1.6, 2.2)]
    [InlineData(3, 3, 2, 4, null, 2.4, 1.5, 1.0)]
    [InlineData(4, 2, 1, 1, null, 1.7, 1.2, 1.0)]
    public async Task CalculateAsync_GivenValidInput_ShouldSetCorrectlyCalculationMultipliers(
        int vehicleTariffTypeId,
        int municipalityId,
        int vehiclePurposeId,
        int vehicleTypeId,
        int? ownerAgeId,
        decimal expectedEngineVolumeMultiplier,
        decimal expectedMunicipalityMultiplier,
        decimal expectedAgeGroupMultiplier)

    {
        Mock<IVehicleInfoRepository> vehicleRepository = new();
        vehicleRepository.Setup(x => x.GetTotalPaidAmountAsync())
            .ReturnsAsync(200_00M);
        vehicleRepository.Setup(x => x.GetTotalPendingAmountAsync())
            .ReturnsAsync(100_000M);
        vehicleRepository.Setup(x => x.GetPolicyCountAsync())
            .ReturnsAsync(200);

        Mock<ITariffTypeRepository> tariffTypeRepository = new();
        List<VehicleTariffType> vehicleTariffs = new List<VehicleTariffType>()
        {
            new VehicleTariffType
            {
                Id  = 2,
                Description = "Mock description",
            },
            new VehicleTariffType
            {
                Id  = 3,
                Description = "Mock description",
            },
            new VehicleTariffType
            {
                Id  = 4,
                Description = "Mock description",
            }
        };

        tariffTypeRepository.Setup(x => x.GetByIdAsync(vehicleTariffTypeId))
            .ReturnsAsync(() => vehicleTariffs.FirstOrDefault(x => x.Id == vehicleTariffTypeId));


        Mock<IVehicleTypeRepository> vehicleTypeRepository = new();
        List<VehicleType> vehicleTypes = new List<VehicleType>()
        {
            new VehicleType
            {
                Id = 2,
                Description = "Mock description"
            },
            new VehicleType
            {
                Id = 3,
                Description = "Mock description"
            },
            new VehicleType
            {
                Id = 4,
                Description = "Mock description"
            }
        };

        vehicleTypeRepository.Setup(x => x.GetByIdAsync(vehicleTypeId))
            .ReturnsAsync(() => vehicleTypes.FirstOrDefault(x => x.Id == vehicleTypeId));

        Mock<IEngineVolumeRepository> engineVolumeRepository = new();
        List<EngineVolume> engineVolumes = new List<EngineVolume>()
        {
            new EngineVolume
            {
                Id  = 2,
                Description = "Mock description",
                Multiplier = 3.5m
            },
            new EngineVolume
            {
                Id  = 3,
                Description = "Mock description",
                Multiplier = 2.4m
            },
            new EngineVolume
            {
                Id  = 4,
                Description = "Mock description",
                Multiplier = 1.7m
            }
        };

        engineVolumeRepository.Setup(x => x.GetByIdAsync(vehicleTariffTypeId))
            .ReturnsAsync(() => engineVolumes.FirstOrDefault(x => x.Id == vehicleTariffTypeId));

        Mock<IPurposeRepository> purposeRepository = new();
        List<VehiclePurpose> vehiclePurposes = new List<VehiclePurpose>()
        {
            new VehiclePurpose
            {
                Id  = 1,
                Description = "Mock description",
            },
            new VehiclePurpose
            {
                Id  = 2,
                Description = "Mock description",
            },
            new VehiclePurpose
            {
                Id  = 3,
                Description = "Mock description",
            },
            new VehiclePurpose
            {
                Id  = 4,
                Description = "Mock description",
            }
        };

        purposeRepository.Setup(x => x.GetByIdAsync(vehiclePurposeId))
            .ReturnsAsync(() => vehiclePurposes.FirstOrDefault(x => x.Id == vehiclePurposeId));

        Mock<IAgeGroupRepository> ageGroupRepository = new();
        List<AgeGroup> ageGroups = new List<AgeGroup>()
        {
             new AgeGroup
            {
                Id  = 1,
                Description = "Mock description",
                Multiplier = 2.2m
            },
            new AgeGroup
            {
                Id  = 2,
                Description = "Mock description",
                Multiplier = 2.2m
            },
            new AgeGroup
            {
                Id  = 3,
                Description = "Mock description",
                Multiplier = 2.5m
            },
            new AgeGroup
            {
                Id  = 4,
                Description = "Mock description",
                Multiplier = 2.6m
            }
        };

        if (ownerAgeId.HasValue)
        {
            ageGroupRepository.Setup(x => x.GetByIdAsync(ownerAgeId!.Value))
                .ReturnsAsync(() => ageGroups.FirstOrDefault(x => x.Id == ownerAgeId));
        }

        Mock<IMunicipalityRepository> municipalityRepository = new();
        List<Municipality> municipalities = new List<Municipality>()
        {
            new Municipality
            {
                Id  = 2,
                Name = "Test name",
                Multiplier = 1.2m
            },
            new Municipality
            {
                Id  = 3,
                Name = "Test name",
                Multiplier = 1.5m
            },
            new Municipality
            {
                Id  = 4,
                Name = "Test name",
                Multiplier = 1.6m
            }
        };

        municipalityRepository.Setup(x => x.GetByIdAsync(municipalityId))
            .ReturnsAsync(() => municipalities.FirstOrDefault(x => x.Id == municipalityId));

        Mock<ICalculationRepository> calculationRepository = new();
        Mock<ILogger<CalculationService>> logger = new();

        CalculationService calculationService = new CalculationService(
            vehicleRepository.Object,
            tariffTypeRepository.Object,
            engineVolumeRepository.Object,
            ageGroupRepository.Object,
            municipalityRepository.Object,
            calculationRepository.Object,
            purposeRepository.Object,
            logger.Object,
            CreateMapper()
        );

        var calculationRequest = new CalculationRequest
        {
            VehicleTariffTypeId = vehicleTariffTypeId,
            MunicipalityId = municipalityId,
            VehiclePurposeId = vehiclePurposeId,
            VehicleTypeId = vehicleTypeId,
            OwnerAgeId = ownerAgeId,
        };

        CalculationResponse calculation = await calculationService.CalculateAsync(calculationRequest);



        Assert.Equal(expectedAgeGroupMultiplier, calculation.AgeMultiplier);
        Assert.Equal(expectedMunicipalityMultiplier, calculation.MunicipalityMultiplier);
        Assert.Equal(expectedEngineVolumeMultiplier, calculation.EngineVolumeMultiplier);
    }

    [Fact]
    public async Task CalculateAsync_GivenInvalidAgeGroup_ShouldThrowException()
    {
        Mock<IVehicleInfoRepository> vehicleRepository = new();
        vehicleRepository.Setup(x => x.GetTotalPaidAmountAsync())
            .ReturnsAsync(200_00M);
        vehicleRepository.Setup(x => x.GetTotalPendingAmountAsync())
            .ReturnsAsync(100_000M);
        vehicleRepository.Setup(x => x.GetPolicyCountAsync())
            .ReturnsAsync(200);

        Mock<ITariffTypeRepository> tariffTypeRepository = new();
        tariffTypeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehicleTariffType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IVehicleTypeRepository> vehicleTypeRepository = new();
        vehicleTypeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehicleType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IEngineVolumeRepository> engineVolumeRepository = new();
        engineVolumeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new EngineVolume
            {
                Id = 2,
                Description = "Mock description",
                Multiplier = 1.02M
            });

        Mock<IPurposeRepository> purposeRepository = new();
        purposeRepository.Setup(x => x.GetByIdAsync(3))
            .ReturnsAsync(new VehiclePurpose
            {
                Id = 3,
                Description = "Mock description"
            });

        Mock<IAgeGroupRepository> ageGroupRepository = new();
        Mock<IMunicipalityRepository> municipalityRepository = new();
        municipalityRepository.Setup(x => x.GetByIdAsync(3))
            .ReturnsAsync(new Municipality
            {
                Id = 3,
                Name = "TestName",
                Multiplier = 1.2m
            });

        Mock<ICalculationRepository> calculationRepository = new();
        Mock<ILogger<CalculationService>> logger = new();

        CalculationService calculationService = new CalculationService(
            vehicleRepository.Object,
            tariffTypeRepository.Object,
            engineVolumeRepository.Object,
            ageGroupRepository.Object,
            municipalityRepository.Object,
            calculationRepository.Object,
            purposeRepository.Object,
            logger.Object,
            CreateMapper()
        );

        var calculationRequest = new CalculationRequest
        {
            VehicleTariffTypeId = 2,
            MunicipalityId = 2,
            VehiclePurposeId = 3,
            VehicleTypeId = 2,
            OwnerAgeId = 1,
        };

        EntityNotFoundException exception = await Assert.ThrowsAsync<EntityNotFoundException>(()
            => calculationService.CalculateAsync(calculationRequest));

        var expectedMessage = "Not results found";

        Assert.NotNull(exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task CalculateAsync_GivenNullAgeGroupParameterWithValidPurposeAndTypeIds_ShouldThrowException()
    {
        Mock<IVehicleInfoRepository> vehicleRepository = new();
        vehicleRepository.Setup(x => x.GetTotalPaidAmountAsync())
            .ReturnsAsync(200_00M);
        vehicleRepository.Setup(x => x.GetTotalPendingAmountAsync())
            .ReturnsAsync(100_000M);
        vehicleRepository.Setup(x => x.GetPolicyCountAsync())
            .ReturnsAsync(200);

        Mock<ITariffTypeRepository> tariffTypeRepository = new();
        tariffTypeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehicleTariffType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IVehicleTypeRepository> vehicleTypeRepository = new();
        vehicleTypeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehicleType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IEngineVolumeRepository> engineVolumeRepository = new();
        engineVolumeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new EngineVolume
            {
                Id = 2,
                Description = "Mock description",
                Multiplier = 1.02M
            });

        Mock<IPurposeRepository> purposeRepository = new();
        purposeRepository.Setup(x => x.GetByIdAsync(3))
            .ReturnsAsync(new VehiclePurpose
            {
                Id = 3,
                Description = "Mock description"
            });

        Mock<IAgeGroupRepository> ageGroupRepository = new();
        Mock<IMunicipalityRepository> municipalityRepository = new();
        municipalityRepository.Setup(x => x.GetByIdAsync(3))
            .ReturnsAsync(new Municipality
            {
                Id = 3,
                Name = "TestName",
                Multiplier = 1.2m
            });

        Mock<ICalculationRepository> calculationRepository = new();
        Mock<ILogger<CalculationService>> logger = new();

        CalculationService calculationService = new CalculationService(
            vehicleRepository.Object,
            tariffTypeRepository.Object,
            engineVolumeRepository.Object,
            ageGroupRepository.Object,
            municipalityRepository.Object,
            calculationRepository.Object,
            purposeRepository.Object,
            logger.Object,
            CreateMapper()
        );

        var calculationRequest = new CalculationRequest
        {
            VehicleTariffTypeId = 2,
            MunicipalityId = 2,
            VehiclePurposeId = 3,
            VehicleTypeId = 2,
            OwnerAgeId = null,
        };

        NullReferenceException exception = await Assert.ThrowsAsync<NullReferenceException>(()
            => calculationService.CalculateAsync(calculationRequest));

        var expectedMessage = "Invalid value";

        Assert.NotNull(exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    //ToDo correct the name
    [Fact]
    public async Task CalculateAsync_GivenNullableAgeGroupId_ShouldReturnExpectedAgeMultiplier()
    {
        Mock<IVehicleInfoRepository> vehicleRepository = new();
        vehicleRepository.Setup(x => x.GetTotalPaidAmountAsync())
            .ReturnsAsync(200_00M);
        vehicleRepository.Setup(x => x.GetTotalPendingAmountAsync())
            .ReturnsAsync(100_000M);
        vehicleRepository.Setup(x => x.GetPolicyCountAsync())
            .ReturnsAsync(200);

        Mock<ITariffTypeRepository> tariffTypeRepository = new();
        tariffTypeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehicleTariffType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IVehicleTypeRepository> vehicleTypeRepository = new();
        vehicleTypeRepository.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(new VehicleType
            {
                Id = 1,
                Description = "Mock description"
            });

        Mock<IEngineVolumeRepository> engineVolumeRepository = new();
        engineVolumeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new EngineVolume
            {
                Id = 2,
                Description = "Mock description",
                Multiplier = 1.02M
            });

        Mock<IPurposeRepository> purposeRepository = new();
        purposeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehiclePurpose
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IAgeGroupRepository> ageGroupRepository = new();
        Mock<IMunicipalityRepository> municipalityRepository = new();
        municipalityRepository.Setup(x => x.GetByIdAsync(3))
            .ReturnsAsync(new Municipality
            {
                Id = 3,
                Name = "TestName",
                Multiplier = 1.2m
            });

        Mock<ICalculationRepository> calculationRepository = new();
        Mock<ILogger<CalculationService>> logger = new();

        CalculationService calculationService = new CalculationService(
            vehicleRepository.Object,
            tariffTypeRepository.Object,
            engineVolumeRepository.Object,
            ageGroupRepository.Object,
            municipalityRepository.Object,
            calculationRepository.Object,
            purposeRepository.Object,
            logger.Object,
            CreateMapper()
        );

        var calculationRequest = new CalculationRequest
        {
            VehicleTariffTypeId = 2,
            MunicipalityId = 3,
            VehiclePurposeId = 2,
            VehicleTypeId = 1,
            OwnerAgeId = null,
        };

        CalculationResponse calculation = await calculationService.CalculateAsync(calculationRequest);

        decimal expectedAgeMultiplier = 1.00m;

        Assert.Equal(expectedAgeMultiplier, calculation.AgeMultiplier);
    }

    [Fact]
    public async Task CalculateAsync_GivenInvalidMunicipalityId_ShouldThrowException()
    {
        Mock<IVehicleInfoRepository> vehicleRepository = new();
        vehicleRepository.Setup(x => x.GetTotalPaidAmountAsync())
            .ReturnsAsync(200_00M);
        vehicleRepository.Setup(x => x.GetTotalPendingAmountAsync())
            .ReturnsAsync(100_000M);
        vehicleRepository.Setup(x => x.GetPolicyCountAsync())
            .ReturnsAsync(200);

        Mock<ITariffTypeRepository> tariffTypeRepository = new();
        tariffTypeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehicleTariffType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IEngineVolumeRepository> engineVolumeRepository = new();
        engineVolumeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new EngineVolume
            {
                Id = 2,
                Description = "Mock description",
                Multiplier = 1.02M
            });

        Mock<IPurposeRepository> purposeRepository = new();
        purposeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehiclePurpose
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IAgeGroupRepository> ageGroupRepository = new();
        Mock<IMunicipalityRepository> municipalityRepository = new();
        municipalityRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new Municipality
            {
                Id = 2,
                Name = "TestName",
                Multiplier = 1.2m
            });

        Mock<ICalculationRepository> calculationRepository = new();
        Mock<ILogger<CalculationService>> logger = new();

        CalculationService calculationService = new CalculationService(
            vehicleRepository.Object,
            tariffTypeRepository.Object,
            engineVolumeRepository.Object,
            ageGroupRepository.Object,
            municipalityRepository.Object,
            calculationRepository.Object,
            purposeRepository.Object,
            logger.Object,
            CreateMapper()
        );


        var calculationRequest = new CalculationRequest
        {
            VehicleTariffTypeId = 2,
            MunicipalityId = 5,
            VehiclePurposeId = 2,
            VehicleTypeId = 2,
            OwnerAgeId = 3,
        };

        EntityNotFoundException exception = await Assert.ThrowsAsync<EntityNotFoundException>(()
            => calculationService.CalculateAsync(calculationRequest));

        var expectedMessage = "Not results found";

        Assert.NotNull(exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task CalculateAsync_GivenInvalidPurposeId_ShouldThrowException()
    {
        Mock<IVehicleInfoRepository> vehicleRepository = new();
        vehicleRepository.Setup(x => x.GetTotalPaidAmountAsync())
            .ReturnsAsync(200_00M);
        vehicleRepository.Setup(x => x.GetTotalPendingAmountAsync())
            .ReturnsAsync(100_000M);
        vehicleRepository.Setup(x => x.GetPolicyCountAsync())
            .ReturnsAsync(200);

        Mock<ITariffTypeRepository> tariffTypeRepository = new();
        tariffTypeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehicleTariffType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IEngineVolumeRepository> engineVolumeRepository = new();
        engineVolumeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new EngineVolume
            {
                Id = 2,
                Description = "Mock description",
                Multiplier = 1.02M
            });

        Mock<IPurposeRepository> purposeRepository = new();
        purposeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehiclePurpose
            {
                Id = 4,
                Description = "Mock description"
            });

        Mock<IAgeGroupRepository> ageGroupRepository = new();
        Mock<IMunicipalityRepository> municipalityRepository = new();
        Mock<ICalculationRepository> calculationRepository = new();
        Mock<ILogger<CalculationService>> logger = new();

        CalculationService calculationService = new CalculationService(
            vehicleRepository.Object,
            tariffTypeRepository.Object,
            engineVolumeRepository.Object,
            ageGroupRepository.Object,
            municipalityRepository.Object,
            calculationRepository.Object,
            purposeRepository.Object,
            logger.Object,
            CreateMapper()
        );


        var calculationRequest = new CalculationRequest
        {
            VehicleTariffTypeId = 2,
            MunicipalityId = 2,
            VehiclePurposeId = 22,
            VehicleTypeId = 2,
            OwnerAgeId = 3,
        };

        NullReferenceException exception = await Assert.ThrowsAsync<NullReferenceException>(()
            => calculationService.CalculateAsync(calculationRequest));

        string expectedMessage = "Invalid value";

        Assert.NotNull(exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task CalculateAsync_GivenInvalidVehicleTariffTypeId_ShouldThrowException()
    {
        Mock<IVehicleInfoRepository> vehicleRepository = new();
        vehicleRepository.Setup(x => x.GetTotalPaidAmountAsync())
            .ReturnsAsync(200_00M);
        vehicleRepository.Setup(x => x.GetTotalPendingAmountAsync())
            .ReturnsAsync(100_000M);
        vehicleRepository.Setup(x => x.GetPolicyCountAsync())
            .ReturnsAsync(200);

        Mock<ITariffTypeRepository> tariffTypeRepository = new();
        tariffTypeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehicleTariffType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IEngineVolumeRepository> engineVolumeRepository = new();
        engineVolumeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new EngineVolume
            {
                Id = 2,
                Description = "Mock description",
                Multiplier = 1.02M
            });

        Mock<IPurposeRepository> purposeRepository = new();
        Mock<IAgeGroupRepository> ageGroupRepository = new();
        Mock<IMunicipalityRepository> municipalityRepository = new();
        Mock<ICalculationRepository> calculationRepository = new();
        Mock<ILogger<CalculationService>> logger = new();

        var calculationRequest = new CalculationRequest
        {
            VehicleTariffTypeId = 19,
            MunicipalityId = 5,
            VehiclePurposeId = 3,
            VehicleTypeId = 2,
            OwnerAgeId = 1,
        };

        CalculationService calculationService = new CalculationService(
            vehicleRepository.Object,
            tariffTypeRepository.Object,
            engineVolumeRepository.Object,
            ageGroupRepository.Object,
            municipalityRepository.Object,
            calculationRepository.Object,
            purposeRepository.Object,
            logger.Object,
            CreateMapper()
        );

        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(()
            => calculationService.CalculateAsync(calculationRequest));

        string expectedMessage = "Not results found";

        Assert.NotNull(exception);
        Assert.Equal(expectedMessage, exception.Message);

    }

    [Fact]
    public async Task CalculateAsync_GivenNoVehicleInfos_ShouldThrowException()
    {
        Mock<IVehicleInfoRepository> vehicleRepository = new();
        Mock<ITariffTypeRepository> tariffTypeRepository = new();
        tariffTypeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehicleTariffType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IVehicleTypeRepository> vehicleTypeRepository = new();
        vehicleTypeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new VehicleType
            {
                Id = 2,
                Description = "Mock description"
            });

        Mock<IEngineVolumeRepository> engineVolumeRepository = new();
        engineVolumeRepository.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new EngineVolume
            {
                Id = 2,
                Description = "Mock description",
                Multiplier = 1.02M
            });

        Mock<IPurposeRepository> purposeRepository = new();
        purposeRepository.Setup(x => x.GetByIdAsync(3))
            .ReturnsAsync(new VehiclePurpose
            {
                Id = 3,
                Description = "Mock description"
            });

        Mock<IAgeGroupRepository> ageGroupRepository = new();
        ageGroupRepository.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(new AgeGroup
            {
                Id = 1,
                Description = "Test description",
                Multiplier = 1.5m
            });

        Mock<IMunicipalityRepository> municipalityRepository = new();
        municipalityRepository.Setup(x => x.GetByIdAsync(3))
            .ReturnsAsync(new Municipality
            {
                Id = 3,
                Name = "TestName",
                Multiplier = 1.2m
            });

        Mock<ICalculationRepository> calculationRepository = new();
        Mock<ILogger<CalculationService>> logger = new();

        CalculationService calculationService = new CalculationService(
            vehicleRepository.Object,
            tariffTypeRepository.Object,
            engineVolumeRepository.Object,
            ageGroupRepository.Object,
            municipalityRepository.Object,
            calculationRepository.Object,
            purposeRepository.Object,
            logger.Object,
            CreateMapper()
        );

        var calculationRequest = new CalculationRequest
        {
            VehicleTariffTypeId = 2,
            MunicipalityId = 3,
            VehiclePurposeId = 3,
            VehicleTypeId = 2,
            OwnerAgeId = 1,
        };

        var exception = await Assert.ThrowsAsync<DivideByZeroException>(()
            => calculationService.CalculateAsync(calculationRequest));

        string expectedMessage = "Attempted to divide by zero.";

        Assert.Equal(expectedMessage, exception.Message);
    }


    private IMapper CreateMapper()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new CalculationProfile());
        });

        IMapper mapper = mappingConfig.CreateMapper();
        return mapper;
    }

}
