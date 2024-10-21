using AutoparkService.Application.DTOs.MaintenanceRecord.Request;
using AutoparkService.Domain.Models;
using Bogus;

namespace AutoparkService.IntegrationTests.DataGenerators;

public static class MaintenanceRecordDataFaker
{
    private static readonly string[] Types =
        ["Oil Change", "Tire Replacement", "Brake Inspection", "Engine Repair", "Battery Check"];

    public static Faker<MaintenanceRecord> MaintenanceRecordFaker => new Faker<MaintenanceRecord>()
        .RuleFor(m => m.Id, _ => Guid.NewGuid())
        .RuleFor(m => m.Type, f => f.PickRandom(Types))
        .RuleFor(m => m.StartDate, f => f.Date.PastDateOnly(5))
        .RuleFor(m => m.EndDate, (f, m) => f.Random.Bool() ? m.StartDate.AddDays(f.Random.Int(1, 30)) : null)
        .RuleFor(m => m.Odometer, f => f.Random.UInt(0, 300_000))
        .RuleFor(
            m => m.ServiceCenter,
            f => f.Company.CompanyName())
        .RuleFor(m => m.Cost, f => f.Finance.Amount(100, 5000))
        .RuleFor(m => m.Vehicle, _ => VehicleDataFaker.VehicleFaker.Generate())
        .RuleFor(m => m.VehicleId, (_, m) => m.Vehicle.Id);

    public static Faker<MaintenanceRecordRequest> MaintenanceRecordRequestFaker => new Faker<MaintenanceRecordRequest>()
        .CustomInstantiator(
            f => new MaintenanceRecordRequest(
                f.PickRandom(Types),
                f.Date.PastDateOnly(5).ToString("O"),
                null!,
                f.Random.UInt(0, 300_000),
                f.Company.CompanyName(),
                f.Finance.Amount(100, 5000),
                Guid.Empty))
        .RuleFor(
            m => m.EndDate,
            (f, m) => f.Random.Bool() ? DateOnly.Parse(m.StartDate).AddDays(f.Random.Int(1, 30)).ToString("O") : null);
}
