using AutoparkService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoparkService.Infrastructure.Contexts.Configurations;

public class PermissionConfiguration
    : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .HasIndex(p => p.Number)
            .IsUnique();

        builder
            .Property(p => p.Number)
            .HasMaxLength(9);
    }
}
