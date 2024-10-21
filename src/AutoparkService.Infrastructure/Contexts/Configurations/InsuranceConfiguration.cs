using AutoparkService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoparkService.Infrastructure.Contexts.Configurations;

public class InsuranceConfiguration
    : IEntityTypeConfiguration<Insurance>
{
    public void Configure(EntityTypeBuilder<Insurance> builder)
    {
        builder
            .HasKey(i => i.Id);

        builder
            .HasIndex(i => new { i.Series, i.Number })
            .IsUnique();

        builder
            .Property(i => i.Series)
            .HasMaxLength(2);

        builder
            .Property(i => i.Number)
            .HasMaxLength(7);

        builder
            .Property(i => i.VehicleType)
            .HasMaxLength(2);

        builder
            .Property(i => i.Provider)
            .HasMaxLength(255);

        builder
            .Property(i => i.Cost)
            .HasPrecision(11, 2);
    }
}
