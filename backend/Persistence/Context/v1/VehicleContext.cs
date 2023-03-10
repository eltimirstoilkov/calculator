using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities.v1;

namespace Persistence.Context.v1;

public class VehicleContext : DbContext
{
    public VehicleContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<VehiclePurpose> VehiclePurposes { get; set; }

    public DbSet<VehicleType> VehicleTypes { get; set; }


    public DbSet<Municipality> Municipalities { get; set; }

    public DbSet<VehicleInfo> VehicleInfos { get; set; }

    public DbSet<VehicleTariffType> VehicleTariffTypes { get; set; }

    public DbSet<EngineVolume> EngineVolumes { get; set; }

    public DbSet<AgeGroup> AgeGroups { get; set; }

    public DbSet<Calculation> Calculations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        VehicleTypeConfiguration(modelBuilder.Entity<VehicleType>());
        PurposeConfiguration(modelBuilder.Entity<VehiclePurpose>());
        MunicipalitiesConfiguration(modelBuilder.Entity<Municipality>());
        TariffTypeConfiguration(modelBuilder.Entity<VehicleTariffType>());
        VehicleInfoConfiguration(modelBuilder.Entity<VehicleInfo>());
        AgeGroupConfiguration(modelBuilder.Entity<AgeGroup>());
        EngineVolumeConfiguration(modelBuilder.Entity<EngineVolume>());

        base.OnModelCreating(modelBuilder);
    }

    private void VehicleTypeConfiguration(EntityTypeBuilder<VehicleType> vehicleType)
    {
        vehicleType
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(30);
    }

    private void PurposeConfiguration(EntityTypeBuilder<VehiclePurpose> purpose)
    {
        purpose
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(100);
    }

    private void MunicipalitiesConfiguration(EntityTypeBuilder<Municipality> municipality)
    {
        municipality
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
    }

    private void TariffTypeConfiguration(EntityTypeBuilder<VehicleTariffType> tariff)
    {
        tariff
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(100);
    }

    private void VehicleInfoConfiguration(EntityTypeBuilder<VehicleInfo> vehicleInfo)
    {
        vehicleInfo
            .Property(x => x.PolicyNumber)
            .IsRequired()
            .HasMaxLength(20);

        vehicleInfo
            .Property(x => x.PaidAmount)
            .HasPrecision(19, 3);

        vehicleInfo
            .Property(x => x.PendingAmount)
            .HasPrecision(19, 3);

        vehicleInfo.Property(x => x.TariffTypeId)
            .HasColumnName("VehicleTariffTypeId");

        vehicleInfo
            .HasOne(x => x.TariffType)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        vehicleInfo
            .HasOne(x => x.VehicleType)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        vehicleInfo
            .HasOne(x => x.VehiclePurpose)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        vehicleInfo
            .HasOne(x => x.Municipality)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        vehicleInfo
            .HasOne(x => x.EngineVolume)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        vehicleInfo
            .HasOne(x => x.AgeGroup)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }

    public void AgeGroupConfiguration(EntityTypeBuilder<AgeGroup> ageGroup)
    {
        ageGroup
            .HasKey(x => x.Id);

        ageGroup
            .Property(x => x.Description)
            .IsRequired().IsUnicode(true)
            .HasMaxLength(100);

        ageGroup
            .Property(x => x.Multiplier)
            .HasPrecision(4, 2);
    }

    public void EngineVolumeConfiguration(EntityTypeBuilder<EngineVolume> ageGroup)
    {
        ageGroup
            .HasKey(x => x.Id);

        ageGroup
            .Property(x => x.Multiplier)
            .HasPrecision(4, 2);
    }
}