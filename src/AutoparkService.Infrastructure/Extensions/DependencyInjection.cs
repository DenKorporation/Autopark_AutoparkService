using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Infrastructure.Contexts;
using AutoparkService.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoparkService.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);
        services.ConfigureRepositories();

        return services;
    }

    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AutoParkDbContext>(
            options =>
                options.UseNpgsql(configuration.GetConnectionString("Database")));

        services.AddScoped<AutoParkDbContextInitializer>();
    }

    private static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITechnicalPassportRepository, TechnicalPassportRepository>();
        services.AddScoped<IInsuranceRepository, InsuranceRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IMaintenanceRecordRepository, MaintenanceRecordRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
    }
}
