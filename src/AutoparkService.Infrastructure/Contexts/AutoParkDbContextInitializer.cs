using AutoparkService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AutoparkService.Infrastructure.Contexts;

public class AutoParkDbContextInitializer(
    ILogger<AutoParkDbContextInitializer> logger,
    AutoParkDbContext context)
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await context.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");

            throw;
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await TrySeedAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");

            throw;
        }
    }

    private static List<Vehicle> GetPreconfiguredVehicles()
    {
        return
        [
            new Vehicle
            {
                Id = new Guid("2E6E4981-3349-4EFC-92BD-2DB1EAF9776A"),
                Odometer = 20_000,
                PurchaseDate = DateOnly.Parse("2023-01-01"),
                Cost = 70_000.00M,
                Status = "Active",
                UserId = Guid.NewGuid(),
            },
        ];
    }

    private static List<TechnicalPassport> GetPreconfiguredTechnicalPassports()
    {
        return
        [
            new TechnicalPassport
            {
                Id = new Guid("936B3326-6532-4947-9831-3BF64D6D862B"),
                Number = "AAA128400",
                FirstName = "Иван",
                FirstNameLatin = "Ivan",
                LastName = "Иванов",
                LastNameLatin = "Ivanov",
                Patronymic = "Иванович",
                Address = "Минская обл., Советский район, г. Минск, ул. Гикало, д. 9",
                IssueDate = DateOnly.Parse("2023-01-01"),
                SAICode = "102-18",
                LicensePlate = "1234AA-7",
                Brand = "Audi",
                Model = "A6",
                CreationYear = 2022,
                Color = "СЕРЕБРИСТЫЙ МЕТАЛЛИК",
                VIN = "WAUZZZ4G7FN123456",
                VehicleType = "Легковой",
                MaxWeight = 1905,
                VehicleId = new Guid("2E6E4981-3349-4EFC-92BD-2DB1EAF9776A"),
            },
        ];
    }

    private static List<Insurance> GetPreconfiguredInsurances()
    {
        return
        [
            new Insurance
            {
                Id = new Guid("BF4BC6C9-8CDA-4132-BF13-6D59625EA09F"),
                Series = "MA",
                Number = "1234567",
                VehicleType = "A4",
                Provider = "ЗАСО \"Белнефтестрах\"",
                IssueDate = DateOnly.Parse("2024-01-01"),
                StartDate = DateOnly.Parse("2024-01-02"),
                EndDate = DateOnly.Parse("2025-01-02"),
                Cost = 30.00M,
                VehicleId = new Guid("2E6E4981-3349-4EFC-92BD-2DB1EAF9776A"),
            },
        ];
    }

    private static List<MaintenanceRecord> GetPreconfiguredMaintenanceRecords()
    {
        return
        [
            new MaintenanceRecord
            {
                Id = new Guid("EC68A6F4-1B7E-4084-8DCA-04BB64D442B8"),
                Type = "technical service",
                StartDate = DateOnly.Parse("2023-05-02"),
                EndDate = DateOnly.Parse("2023-05-04"),
                Odometer = 14_000,
                ServiceCenter = "Service Center",
                Cost = 120.00M,
                VehicleId = new Guid("2E6E4981-3349-4EFC-92BD-2DB1EAF9776A"),
            },
        ];
    }

    private static List<Permission> GetPreconfiguredPermissions()
    {
        return
        [
            new Permission
            {
                Id = new Guid("C278A1C7-6BB3-4029-8212-7C1377A1A5FE"),
                Number = "AB1234567",
                ExpiryDate = DateOnly.Parse("2025-05-01"),
                VehicleId = new Guid("2E6E4981-3349-4EFC-92BD-2DB1EAF9776A"),
            },
        ];
    }

    private async Task TrySeedAsync(CancellationToken cancellationToken = default)
    {
        if (!await context.Vehicles.AnyAsync(cancellationToken))
        {
            await context.Vehicles.AddRangeAsync(GetPreconfiguredVehicles(), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.TechnicalPassports.AnyAsync(cancellationToken))
        {
            await context.TechnicalPassports.AddRangeAsync(GetPreconfiguredTechnicalPassports(), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Insurances.AnyAsync(cancellationToken))
        {
            await context.Insurances.AddRangeAsync(GetPreconfiguredInsurances(), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.MaintenanceRecords.AnyAsync(cancellationToken))
        {
            await context.MaintenanceRecords.AddRangeAsync(GetPreconfiguredMaintenanceRecords(), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Permissions.AnyAsync(cancellationToken))
        {
            await context.Permissions.AddRangeAsync(GetPreconfiguredPermissions(), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
