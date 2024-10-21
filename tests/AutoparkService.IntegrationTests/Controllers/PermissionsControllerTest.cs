using System.Net;
using AutoparkService.Application.UseCases.Queries.Permissions.GetAll;
using AutoparkService.Domain.Models;
using AutoparkService.IntegrationTests.DataGenerators;
using AutoparkService.IntegrationTests.Responses;
using AutoparkService.IntegrationTests.RestApis.Interfaces;
using FluentAssertions;
using Refit;

namespace AutoparkService.IntegrationTests.Controllers;

public class PermissionControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly IPermissionsApi _permissionsApi;
    private readonly Func<Task> _resetDatabase;

    public PermissionControllerTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _permissionsApi = RestService.For<IPermissionsApi>(Client);
        _resetDatabase = factory.ResetDatabase;
    }

    [Fact]
    public async Task GetAllPermissions_PermissionsExist_ReturnsPagedList()
    {
        var vehicle = await CreateVehicleAsync();
        await CreatePermissionAsync(vehicle);

        var response =
            await _permissionsApi.GetAllPermissionsAsync(
                new FilterPermissionsRequest(
                    1,
                    5,
                    null,
                    null,
                    null,
                    null,
                    null));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var permissionResponse = response.Content!;
        permissionResponse.Should().NotBeNull();
        permissionResponse.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetPermissionById_PermissionExist_ReturnsPermission()
    {
        var vehicle = await CreateVehicleAsync();
        var permission = await CreatePermissionAsync(vehicle);

        var response =
            await _permissionsApi.GetPermissionByIdAsync(permission.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var permissionResponse = response.Content;
        permissionResponse.Should().NotBeNull();
        permissionResponse!.Id.Should().Be(permission.Id);
    }

    [Fact]
    public async Task GetPermissionById_PermissionNotExists_ReturnsNotFound()
    {
        var response = await _permissionsApi.GetPermissionByIdAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("Permission.NotFound");
    }

    [Fact]
    public async Task CreatePermission_PermissionNotExists_ReturnsCreatedWithPermission()
    {
        var vehicle = await CreateVehicleAsync();
        var permission = PermissionDataFaker.PermissionRequestFaker
            .RuleFor(p => p.VehicleId, _ => vehicle.Id)
            .Generate();

        var response = await _permissionsApi.CreatePermissionAsync(permission);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var permissionResponse = response.Content;
        permissionResponse.Should().NotBeNull();
        permissionResponse!.Number.Should().Be(permission.Number);
    }

    [Fact]
    public async Task UpdatePermission_PermissionsExist_ReturnsOkWithUpdatedData()
    {
        var vehicle = await CreateVehicleAsync();
        var permission = await CreatePermissionAsync(vehicle);

        var permissionRequest = PermissionDataFaker.PermissionRequestFaker
            .RuleFor(p => p.VehicleId, _ => vehicle.Id)
            .Generate();

        var response = await _permissionsApi.UpdatePermissionAsync(permission.Id, permissionRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var permissionResponse = response.Content;
        permissionResponse.Should().NotBeNull();
        permissionResponse!.Number.Should().Be(permissionRequest.Number);
    }

    [Fact]
    public async Task UpdatePermission_PermissionNotExist_ReturnsNotFound()
    {
        var permissionRequest = PermissionDataFaker.PermissionRequestFaker
            .RuleFor(p => p.VehicleId, _ => Guid.NewGuid())
            .Generate();

        var response = await _permissionsApi.UpdatePermissionAsync(Guid.NewGuid(), permissionRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("Permission.NotFound");
    }

    [Fact]
    public async Task DeletePermission_PermissionsExist_ReturnsNoContent()
    {
        var vehicle = await CreateVehicleAsync();
        var permission = await CreatePermissionAsync(vehicle);

        var response = await _permissionsApi.DeletePermissionsAsync(permission.Id);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeletePermission_PermissionNotExist_ReturnsNotFound()
    {
        var response = await _permissionsApi.DeletePermissionsAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("Permission.NotFound");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();

    private async Task<Vehicle> CreateVehicleAsync()
    {
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        await AddEntityToDbAsync(vehicle);

        return vehicle;
    }

    private async Task<Permission> CreatePermissionAsync(Vehicle vehicle)
    {
        var permission = PermissionDataFaker.PermissionFaker
            .RuleFor(p => p.VehicleId, _ => vehicle.Id)
            .Generate();

        await AddEntityToDbAsync(permission);

        return permission;
    }
}
