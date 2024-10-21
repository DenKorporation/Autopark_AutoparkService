using System.Net;
using AutoparkService.Application.UseCases.Queries.Vehicles.GetAll;
using AutoparkService.Domain.Models;
using AutoparkService.IntegrationTests.DataGenerators;
using AutoparkService.IntegrationTests.Responses;
using AutoparkService.IntegrationTests.RestApis.Interfaces;
using FluentAssertions;
using Refit;

namespace AutoparkService.IntegrationTests.Controllers;

public class VehiclesControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly IVehiclesApi _vehiclesApi;
    private readonly Func<Task> _resetDatabase;

    public VehiclesControllerTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _vehiclesApi = RestService.For<IVehiclesApi>(Client);
        _resetDatabase = factory.ResetDatabase;
    }

    [Fact]
    public async Task GetAllVehicles_VehiclesExist_ReturnsPagedList()
    {
        await CreateVehicleAsync();

        var response =
            await _vehiclesApi.GetAllVehiclesAsync(
                new FilterVehiclesRequest(
                    1,
                    5,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var vehicleResponse = response.Content!;
        vehicleResponse.Should().NotBeNull();
        vehicleResponse.Items.Should().NotBeEmpty();
    }

    // db contain preconfigured vehicle
    [Fact]
    public async Task GetVehicleById_VehicleExist_ReturnsVehicle()
    {
        var vehicle = await CreateVehicleAsync();

        var response =
            await _vehiclesApi.GetVehicleByIdAsync(vehicle.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var vehicleResponse = response.Content;
        vehicleResponse.Should().NotBeNull();
        vehicleResponse!.Id.Should().Be(vehicle.Id);
    }

    [Fact]
    public async Task GetVehicleById_VehicleNotExists_ReturnsNotFound()
    {
        var response = await _vehiclesApi.GetVehicleByIdAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("Vehicle.NotFound");
    }

    [Fact]
    public async Task CreateVehicle_VehicleNotExists_ReturnsCreatedWithVehicle()
    {
        var vehicle = VehicleDataFaker.VehicleRequestFaker.Generate();

        var response = await _vehiclesApi.CreateVehicleAsync(vehicle);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var vehicleResponse = response.Content;
        vehicleResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateVehicle_VehiclesExist_ReturnsOkWithUpdatedData()
    {
        var vehicle = await CreateVehicleAsync();

        var vehicleRequest = VehicleDataFaker.VehicleRequestFaker.Generate();

        var response = await _vehiclesApi.UpdateVehicleAsync(vehicle.Id, vehicleRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var vehicleResponse = response.Content;
        vehicleResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateVehicle_VehicleNotExist_ReturnsNotFound()
    {
        var vehicle = VehicleDataFaker.VehicleRequestFaker.Generate();

        var response = await _vehiclesApi.UpdateVehicleAsync(Guid.NewGuid(), vehicle);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("Vehicle.NotFound");
    }

    [Fact]
    public async Task DeleteVehicle_VehiclesExist_ReturnsNoContent()
    {
        var vehicle = await CreateVehicleAsync();

        var response = await _vehiclesApi.DeleteVehiclesAsync(vehicle.Id);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteVehicle_VehicleNotExist_ReturnsNotFound()
    {
        var response = await _vehiclesApi.DeleteVehiclesAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("Vehicle.NotFound");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();

    private async Task<Vehicle> CreateVehicleAsync()
    {
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        await AddEntityToDbAsync(vehicle);

        return vehicle;
    }
}
