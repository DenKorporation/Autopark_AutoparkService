using AutoparkService.Application.DTOs.TechnicalPassport.Request;
using AutoparkService.Domain.Models;
using Bogus;

namespace AutoparkService.IntegrationTests.DataGenerators;

public static class TechnicalPassportDataFaker
{
    private static readonly string[] Types =
        ["Sedan", "Truck", "SUV", "Motorcycle", "Bus"];

    public static Faker<TechnicalPassport> TechnicalPassportFaker => new Faker<TechnicalPassport>()
        .RuleFor(i => i.Id, _ => Guid.NewGuid())
        .RuleFor(
            tp => tp.Number,
            f => f.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.String2(6, "0123456789"))
        .RuleFor(tp => tp.FirstName, f => f.Name.FirstName())
        .RuleFor(tp => tp.FirstNameLatin, f => f.Name.FirstName().ToUpper())
        .RuleFor(tp => tp.LastName, f => f.Name.LastName())
        .RuleFor(tp => tp.LastNameLatin, f => f.Name.LastName().ToUpper())
        .RuleFor(tp => tp.Patronymic, f => f.Name.FirstName())
        .RuleFor(tp => tp.Address, f => f.Address.FullAddress())
        .RuleFor(tp => tp.IssueDate, f => f.Date.PastDateOnly(10))
        .RuleFor(tp => tp.SAICode, f => f.Random.String2(3, "0123456789") + "-" + f.Random.String2(2, "0123456789"))
        .RuleFor(
            tp => tp.LicensePlate,
            f => f.Random.String2(4, "0123456789") + f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + "-" +
                 f.Random.String2(1, "0123456789"))
        .RuleFor(tp => tp.Brand, f => f.Vehicle.Manufacturer())
        .RuleFor(tp => tp.Model, f => f.Vehicle.Model())
        .RuleFor(tp => tp.CreationYear, f => (uint)f.Date.Past(30).Year)
        .RuleFor(tp => tp.Color, f => f.Commerce.Color())
        .RuleFor(tp => tp.VIN, f => f.Random.String2(17, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"))
        .RuleFor(tp => tp.VehicleType, f => f.PickRandom(Types))
        .RuleFor(tp => tp.MaxWeight, f => f.Random.UInt(500, 3000))
        .RuleFor(i => i.Vehicle, _ => VehicleDataFaker.VehicleFaker.Generate())
        .RuleFor(i => i.VehicleId, (_, i) => i.Vehicle.Id);

    public static Faker<TechnicalPassportRequest> TechnicalPassportRequestFaker => new Faker<TechnicalPassportRequest>()
        .CustomInstantiator(
            f => new TechnicalPassportRequest(
                f.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.String2(6, "0123456789"),
                f.Name.FirstName(),
                f.Name.FirstName().ToUpper(),
                f.Name.LastName(),
                f.Name.LastName().ToUpper(),
                f.Name.FirstName(),
                f.Address.FullAddress(),
                f.Date.PastDateOnly(10).ToString("O"),
                f.Random.String2(3, "0123456789") + "-" + f.Random.String2(2, "0123456789"),
                f.Random.String2(4, "0123456789") + f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + "-" + f.Random.String2(1, "0123456789"),
                f.Vehicle.Manufacturer(),
                f.Vehicle.Model(),
                (uint)f.Date.Past(30).Year,
                f.Commerce.Color(),
                f.Random.String2(17, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"),
                f.PickRandom(Types),
                f.Random.UInt(500, 3000),
                Guid.Empty));
}
