using AutoparkService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoparkService.Infrastructure.Contexts.Configurations;

public class TechnicalPassportConfiguration
    : IEntityTypeConfiguration<TechnicalPassport>
{
    public void Configure(EntityTypeBuilder<TechnicalPassport> builder)
    {
        builder
            .HasKey(tp => tp.Id);

        builder
            .HasIndex(tp => tp.Number)
            .IsUnique();

        builder
            .HasIndex(tp => tp.LicensePlate)
            .IsUnique();

        builder
            .HasIndex(tp => tp.VIN)
            .IsUnique();

        builder
            .Property(tp => tp.Number)
            .HasMaxLength(9);

        builder
            .Property(tp => tp.FirstName)
            .HasMaxLength(50);

        builder
            .Property(tp => tp.FirstNameLatin)
            .HasMaxLength(50);

        builder
            .Property(tp => tp.LastName)
            .HasMaxLength(50);

        builder
            .Property(tp => tp.LastNameLatin)
            .HasMaxLength(50);

        builder
            .Property(tp => tp.Patronymic)
            .HasMaxLength(50);

        builder
            .Property(tp => tp.Address)
            .HasMaxLength(255);

        builder
            .Property(tp => tp.SAICode)
            .HasMaxLength(6);

        builder
            .Property(tp => tp.LicensePlate)
            .HasMaxLength(8);

        builder
            .Property(tp => tp.Brand)
            .HasMaxLength(30);

        builder
            .Property(tp => tp.Model)
            .HasMaxLength(50);

        builder
            .Property(tp => tp.Color)
            .HasMaxLength(100);

        builder
            .Property(tp => tp.VIN)
            .HasMaxLength(17);

        builder
            .Property(tp => tp.VehicleType)
            .HasMaxLength(50);
    }
}
