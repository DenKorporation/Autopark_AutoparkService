using AutoparkService.Application.DTOs.Vehicle.Request;
using AutoparkService.Domain.Models;
using Bogus;

namespace AutoparkService.IntegrationTests.DataGenerators;

public static class VehicleDataFaker
{
    private static readonly string[] Statuses =
    [
        "Active", "Inactive", "Under Repair", "Decommissioned"
    ];

    public static Faker<Vehicle> VehicleFaker => new Faker<Vehicle>()
        .RuleFor(v => v.Id, _ => Guid.NewGuid())
        .RuleFor(v => v.Odometer, f => f.Random.UInt(0, 300_000))
        .RuleFor(v => v.PurchaseDate, f => f.Date.PastDateOnly(10))
        .RuleFor(v => v.Cost, f => f.Finance.Amount(5000, 50_000))
        .RuleFor(v => v.Status, f => f.PickRandom(Statuses))
        .RuleFor(v => v.UserId, _ => Guid.NewGuid())
        .RuleFor(v => v.TechnicalPassport, _ => null)
        .RuleFor(v => v.Permission, _ => null)
        .RuleFor(v => v.Insurances, _ => new List<Insurance>())
        .RuleFor(v => v.MaintenanceRecords, _ => new List<MaintenanceRecord>());

    public static Faker<VehicleRequest> VehicleRequestFaker => new Faker<VehicleRequest>()
        .CustomInstantiator(
            f => new VehicleRequest(
                f.Random.UInt(0, 300_000),
                f.Date.PastDateOnly(10).ToString("O"),
                f.Finance.Amount(5000, 50_000),
                f.PickRandom(Statuses),
                Guid.NewGuid()));
}
