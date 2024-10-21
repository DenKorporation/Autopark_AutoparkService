using System.Net;
using AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetAll;
using AutoparkService.Domain.Models;
using AutoparkService.IntegrationTests.DataGenerators;
using AutoparkService.IntegrationTests.Responses;
using AutoparkService.IntegrationTests.RestApis.Interfaces;
using FluentAssertions;
using Refit;

namespace AutoparkService.IntegrationTests.Controllers;

public class MaintenanceRecordControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly IMaintenanceRecordsApi _maintenanceRecordsApi;
    private readonly Func<Task> _resetDatabase;

    public MaintenanceRecordControllerTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _maintenanceRecordsApi = RestService.For<IMaintenanceRecordsApi>(Client);
        _resetDatabase = factory.ResetDatabase;
    }

    [Fact]
    public async Task GetAllMaintenanceRecords_MaintenanceRecordsExist_ReturnsPagedList()
    {
        var vehicle = await CreateVehicleAsync();
        await CreateMaintenanceRecordAsync(vehicle);

        var response =
            await _maintenanceRecordsApi.GetAllMaintenanceRecordsAsync(
                new FilterMaintenanceRecordsRequest(
                    1,
                    5,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var maintenanceRecordResponse = response.Content!;
        maintenanceRecordResponse.Should().NotBeNull();
        maintenanceRecordResponse.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetMaintenanceRecordById_MaintenanceRecordExist_ReturnsMaintenanceRecord()
    {
        var vehicle = await CreateVehicleAsync();
        var maintenanceRecord = await CreateMaintenanceRecordAsync(vehicle);

        var response =
            await _maintenanceRecordsApi.GetMaintenanceRecordByIdAsync(maintenanceRecord.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var maintenanceRecordResponse = response.Content;
        maintenanceRecordResponse.Should().NotBeNull();
        maintenanceRecordResponse!.Id.Should().Be(maintenanceRecord.Id);
    }

    [Fact]
    public async Task GetMaintenanceRecordById_MaintenanceRecordNotExists_ReturnsNotFound()
    {
        var response = await _maintenanceRecordsApi.GetMaintenanceRecordByIdAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("MaintenanceRecord.NotFound");
    }

    [Fact]
    public async Task CreateMaintenanceRecord_MaintenanceRecordNotExists_ReturnsCreatedWithMaintenanceRecord()
    {
        var vehicle = await CreateVehicleAsync();
        var maintenanceRecord = MaintenanceRecordDataFaker.MaintenanceRecordRequestFaker
            .RuleFor(mr => mr.VehicleId, _ => vehicle.Id)
            .Generate();

        var response = await _maintenanceRecordsApi.CreateMaintenanceRecordAsync(maintenanceRecord);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var maintenanceRecordResponse = response.Content;
        maintenanceRecordResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateMaintenanceRecord_MaintenanceRecordsExist_ReturnsOkWithUpdatedData()
    {
        var vehicle = await CreateVehicleAsync();
        var maintenanceRecord = await CreateMaintenanceRecordAsync(vehicle);

        var maintenanceRecordRequest = MaintenanceRecordDataFaker.MaintenanceRecordRequestFaker
            .RuleFor(mr => mr.VehicleId, _ => vehicle.Id)
            .Generate();

        var response = await _maintenanceRecordsApi.UpdateMaintenanceRecordAsync(maintenanceRecord.Id, maintenanceRecordRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var maintenanceRecordResponse = response.Content;
        maintenanceRecordResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateMaintenanceRecord_MaintenanceRecordNotExist_ReturnsNotFound()
    {
        var maintenanceRecordRequest = MaintenanceRecordDataFaker.MaintenanceRecordRequestFaker
            .RuleFor(mr => mr.VehicleId, _ => Guid.NewGuid())
            .Generate();

        var response = await _maintenanceRecordsApi.UpdateMaintenanceRecordAsync(Guid.NewGuid(), maintenanceRecordRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("MaintenanceRecord.NotFound");
    }

    [Fact]
    public async Task DeleteMaintenanceRecord_MaintenanceRecordsExist_ReturnsNoContent()
    {
        var vehicle = await CreateVehicleAsync();
        var maintenanceRecord = await CreateMaintenanceRecordAsync(vehicle);

        var response = await _maintenanceRecordsApi.DeleteMaintenanceRecordsAsync(maintenanceRecord.Id);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteMaintenanceRecord_MaintenanceRecordNotExist_ReturnsNotFound()
    {
        var response = await _maintenanceRecordsApi.DeleteMaintenanceRecordsAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("MaintenanceRecord.NotFound");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();

    private async Task<Vehicle> CreateVehicleAsync()
    {
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        await AddEntityToDbAsync(vehicle);

        return vehicle;
    }

    private async Task<MaintenanceRecord> CreateMaintenanceRecordAsync(Vehicle vehicle)
    {
        var maintenanceRecord = MaintenanceRecordDataFaker.MaintenanceRecordFaker
            .RuleFor(mr => mr.VehicleId, _ => vehicle.Id)
            .Generate();

        await AddEntityToDbAsync(maintenanceRecord);

        return maintenanceRecord;
    }
}
