using AutoparkService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoparkService.Infrastructure.Contexts.Configurations;

public class VehicleConfiguration
    : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder
            .HasKey(v => v.Id);

        builder
            .Property(v => v.Cost)
            .HasPrecision(11, 2);

        builder
            .Property(v => v.Status)
            .HasMaxLength(50);

        builder
            .HasOne(v => v.TechnicalPassport)
            .WithOne(tp => tp.Vehicle)
            .HasForeignKey<TechnicalPassport>(tp => tp.VehicleId);

        builder
            .HasOne(v => v.Permission)
            .WithOne(p => p.Vehicle)
            .HasForeignKey<Permission>(p => p.VehicleId);

        builder
            .HasMany(v => v.MaintenanceRecords)
            .WithOne(mr => mr.Vehicle)
            .HasForeignKey(mr => mr.VehicleId);

        builder
            .HasMany(v => v.Insurances)
            .WithOne(i => i.Vehicle)
            .HasForeignKey(i => i.VehicleId);
    }
}
