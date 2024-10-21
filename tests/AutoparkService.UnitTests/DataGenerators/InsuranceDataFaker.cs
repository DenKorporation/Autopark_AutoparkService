using AutoparkService.Application.DTOs.Insurance.Request;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Domain.Models;
using Bogus;

namespace AutoparkService.UnitTests.DataGenerators;

public static class InsuranceDataFaker
{
    public static Faker<Insurance> InsuranceFaker => new Faker<Insurance>()
        .RuleFor(i => i.Id, _ => Guid.NewGuid())
        .RuleFor(i => i.Series, f => f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
        .RuleFor(i => i.Number, f => f.Random.ReplaceNumbers("#######"))
        .RuleFor(
            i => i.VehicleType,
            f => f.Random.String2(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.Number(0, 9))
        .RuleFor(i => i.Provider, f => f.Company.CompanyName())
        .RuleFor(i => i.IssueDate, f => f.Date.PastDateOnly(5))
        .RuleFor(i => i.StartDate, (f, i) => i.IssueDate.AddDays(f.Random.Int(1, 30)))
        .RuleFor(i => i.EndDate, (_, i) => i.StartDate.AddYears(1))
        .RuleFor(i => i.Cost, f => f.Finance.Amount(100, 1000))
        .RuleFor(i => i.Vehicle, _ => VehicleDataFaker.VehicleFaker.Generate())
        .RuleFor(i => i.VehicleId, (_, i) => i.Vehicle.Id);

    public static Faker<InsuranceResponse> InsuranceResponseFaker => new Faker<InsuranceResponse>()
        .CustomInstantiator(
            f => new InsuranceResponse(
                Guid.NewGuid(),
                f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"),
                f.Random.ReplaceNumbers("#######"),
                f.Random.String2(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.Number(0, 9),
                f.Company.CompanyName(),
                f.Date.PastDateOnly(5),
                default,
                default,
                f.Finance.Amount(100, 1000),
                Guid.Empty,
                f.Random.Bool()))
        .RuleFor(i => i.StartDate, (f, i) => i.IssueDate.AddDays(f.Random.Int(1, 30)))
        .RuleFor(i => i.EndDate, (_, i) => i.StartDate.AddYears(1));

    public static Faker<InsuranceRequest> InsuranceRequestFaker => new Faker<InsuranceRequest>()
        .CustomInstantiator(
            f => new InsuranceRequest(
                f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"),
                f.Random.ReplaceNumbers("#######"),
                f.Random.String2(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.Number(0, 9),
                f.Company.CompanyName(),
                f.Date.PastDateOnly(5).ToString(),
                null!,
                null!,
                f.Finance.Amount(100, 1000),
                Guid.Empty))
        .RuleFor(i => i.StartDate, (f, i) => DateOnly.Parse(i.IssueDate).AddDays(f.Random.Int(1, 30)).ToString())
        .RuleFor(i => i.EndDate, (_, i) => DateOnly.Parse(i.StartDate).AddYears(1).ToString());
}
