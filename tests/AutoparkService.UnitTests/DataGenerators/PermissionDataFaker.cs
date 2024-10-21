using AutoparkService.Application.DTOs.Permission.Request;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Domain.Models;
using Bogus;

namespace AutoparkService.UnitTests.DataGenerators;

public static class PermissionDataFaker
{
    public static Faker<Permission> PermissionFaker => new Faker<Permission>()
        .RuleFor(p => p.Id, _ => Guid.NewGuid())
        .RuleFor(
            p => p.Number,
            f => f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.ReplaceNumbers("#######"))
        .RuleFor(p => p.ExpiryDate, f => f.Date.FutureDateOnly(5))
        .RuleFor(p => p.Vehicle, _ => VehicleDataFaker.VehicleFaker.Generate())
        .RuleFor(p => p.VehicleId, (_, p) => p.Vehicle.Id);

    public static Faker<PermissionResponse> PermissionResponseFaker => new Faker<PermissionResponse>()
        .CustomInstantiator(
            f => new PermissionResponse(
                Guid.NewGuid(),
                f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.ReplaceNumbers("#######"),
                f.Date.FutureDateOnly(5),
                Guid.Empty,
                f.Random.Bool()));

    public static Faker<PermissionRequest> PermissionRequestFaker => new Faker<PermissionRequest>()
        .CustomInstantiator(
            f => new PermissionRequest(
                f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.ReplaceNumbers("#######"),
                f.Date.FutureDateOnly(5).ToString(),
                Guid.Empty));
}
