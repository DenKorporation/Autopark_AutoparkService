using System.Reflection;
using AutoparkService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoparkService.Infrastructure.Contexts;

public class AutoParkDbContext(DbContextOptions<AutoParkDbContext> options)
    : DbContext(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<TechnicalPassport> TechnicalPassports { get; set; }
    public DbSet<Insurance> Insurances { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
