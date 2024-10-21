using System.Net;
using AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetAll;
using AutoparkService.Domain.Models;
using AutoparkService.IntegrationTests.DataGenerators;
using AutoparkService.IntegrationTests.Responses;
using AutoparkService.IntegrationTests.RestApis.Interfaces;
using FluentAssertions;
using Refit;

namespace AutoparkService.IntegrationTests.Controllers;

public class TechnicalPassportControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly ITechnicalPassportsApi _technicalPassportsApi;
    private readonly Func<Task> _resetDatabase;

    public TechnicalPassportControllerTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _technicalPassportsApi = RestService.For<ITechnicalPassportsApi>(Client);
        _resetDatabase = factory.ResetDatabase;
    }

    [Fact]
    public async Task GetAllTechnicalPassports_TechnicalPassportsExist_ReturnsPagedList()
    {
        var vehicle = await CreateVehicleAsync();
        await CreateTechnicalPassportAsync(vehicle);

        var response =
            await _technicalPassportsApi.GetAllTechnicalPassportsAsync(
                new FilterTechnicalPassportsRequest(
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
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var technicalPassportResponse = response.Content!;
        technicalPassportResponse.Should().NotBeNull();
        technicalPassportResponse.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetTechnicalPassportById_TechnicalPassportExist_ReturnsTechnicalPassport()
    {
        var vehicle = await CreateVehicleAsync();
        var technicalPassport = await CreateTechnicalPassportAsync(vehicle);

        var response =
            await _technicalPassportsApi.GetTechnicalPassportByIdAsync(technicalPassport.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var technicalPassportResponse = response.Content;
        technicalPassportResponse.Should().NotBeNull();
        technicalPassportResponse!.Id.Should().Be(technicalPassport.Id);
    }

    [Fact]
    public async Task GetTechnicalPassportById_TechnicalPassportNotExists_ReturnsNotFound()
    {
        var response = await _technicalPassportsApi.GetTechnicalPassportByIdAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("TechnicalPassport.NotFound");
    }

    [Fact]
    public async Task CreateTechnicalPassport_TechnicalPassportNotExists_ReturnsCreatedWithTechnicalPassport()
    {
        var vehicle = await CreateVehicleAsync();
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportRequestFaker
            .RuleFor(tp => tp.VehicleId, _ => vehicle.Id)
            .Generate();

        var response = await _technicalPassportsApi.CreateTechnicalPassportAsync(technicalPassport);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var technicalPassportResponse = response.Content;
        technicalPassportResponse.Should().NotBeNull();
        technicalPassportResponse!.Number.Should().Be(technicalPassport.Number);
    }

    [Fact]
    public async Task UpdateTechnicalPassport_TechnicalPassportsExist_ReturnsOkWithUpdatedData()
    {
        var vehicle = await CreateVehicleAsync();
        var technicalPassport = await CreateTechnicalPassportAsync(vehicle);

        var technicalPassportRequest = TechnicalPassportDataFaker.TechnicalPassportRequestFaker
            .RuleFor(tp => tp.VehicleId, _ => vehicle.Id)
            .Generate();

        var response = await _technicalPassportsApi.UpdateTechnicalPassportAsync(
            technicalPassport.Id,
            technicalPassportRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var technicalPassportResponse = response.Content;
        technicalPassportResponse.Should().NotBeNull();
        technicalPassportResponse!.Number.Should().Be(technicalPassportRequest.Number);
    }

    [Fact]
    public async Task UpdateTechnicalPassport_TechnicalPassportNotExist_ReturnsNotFound()
    {
        var technicalPassportRequest = TechnicalPassportDataFaker.TechnicalPassportRequestFaker
            .RuleFor(tp => tp.VehicleId, _ => Guid.NewGuid())
            .Generate();

        var response =
            await _technicalPassportsApi.UpdateTechnicalPassportAsync(Guid.NewGuid(), technicalPassportRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("TechnicalPassport.NotFound");
    }

    [Fact]
    public async Task DeleteTechnicalPassport_TechnicalPassportsExist_ReturnsNoContent()
    {
        var vehicle = await CreateVehicleAsync();
        var technicalPassport = await CreateTechnicalPassportAsync(vehicle);

        var response = await _technicalPassportsApi.DeleteTechnicalPassportsAsync(technicalPassport.Id);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteTechnicalPassport_TechnicalPassportNotExist_ReturnsNotFound()
    {
        var response = await _technicalPassportsApi.DeleteTechnicalPassportsAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("TechnicalPassport.NotFound");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();

    private async Task<Vehicle> CreateVehicleAsync()
    {
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        await AddEntityToDbAsync(vehicle);

        return vehicle;
    }

    private async Task<TechnicalPassport> CreateTechnicalPassportAsync(Vehicle vehicle)
    {
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker
            .RuleFor(tp => tp.VehicleId, _ => vehicle.Id)
            .Generate();

        await AddEntityToDbAsync(technicalPassport);

        return technicalPassport;
    }
}
