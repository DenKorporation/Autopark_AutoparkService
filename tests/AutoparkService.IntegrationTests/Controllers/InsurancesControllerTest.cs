using System.Net;
using AutoparkService.Application.UseCases.Queries.Insurances.GetAll;
using AutoparkService.Domain.Models;
using AutoparkService.IntegrationTests.DataGenerators;
using AutoparkService.IntegrationTests.Responses;
using AutoparkService.IntegrationTests.RestApis.Interfaces;
using FluentAssertions;
using Refit;

namespace AutoparkService.IntegrationTests.Controllers;

public class InsuranceControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly IInsurancesApi _insurancesApi;
    private readonly Func<Task> _resetDatabase;

    public InsuranceControllerTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _insurancesApi = RestService.For<IInsurancesApi>(Client);
        _resetDatabase = factory.ResetDatabase;
    }

    [Fact]
    public async Task GetAllInsurances_InsurancesExist_ReturnsPagedList()
    {
        var vehicle = await CreateVehicleAsync();
        await CreateInsuranceAsync(vehicle);

        var response =
            await _insurancesApi.GetAllInsurancesAsync(
                new FilterInsurancesRequest(
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
                    null));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var insuranceResponse = response.Content!;
        insuranceResponse.Should().NotBeNull();
        insuranceResponse.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetInsuranceById_InsuranceExist_ReturnsInsurance()
    {
        var vehicle = await CreateVehicleAsync();
        var insurance = await CreateInsuranceAsync(vehicle);

        var response =
            await _insurancesApi.GetInsuranceByIdAsync(insurance.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var insuranceResponse = response.Content;
        insuranceResponse.Should().NotBeNull();
        insuranceResponse!.Id.Should().Be(insurance.Id);
    }

    [Fact]
    public async Task GetInsuranceById_InsuranceNotExists_ReturnsNotFound()
    {
        var response = await _insurancesApi.GetInsuranceByIdAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("Insurance.NotFound");
    }

    [Fact]
    public async Task CreateInsurance_InsuranceNotExists_ReturnsCreatedWithInsurance()
    {
        var vehicle = await CreateVehicleAsync();
        var insurance = InsuranceDataFaker.InsuranceRequestFaker
            .RuleFor(i => i.VehicleId, _ => vehicle.Id)
            .Generate();

        var response = await _insurancesApi.CreateInsuranceAsync(insurance);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var insuranceResponse = response.Content;
        insuranceResponse.Should().NotBeNull();
        insuranceResponse!.Number.Should().Be(insurance.Number);
    }

    [Fact]
    public async Task UpdateInsurance_InsurancesExist_ReturnsOkWithUpdatedData()
    {
        var vehicle = await CreateVehicleAsync();
        var insurance = await CreateInsuranceAsync(vehicle);

        var insuranceRequest = InsuranceDataFaker.InsuranceRequestFaker
            .RuleFor(i => i.VehicleId, _ => vehicle.Id)
            .Generate();

        var response = await _insurancesApi.UpdateInsuranceAsync(insurance.Id, insuranceRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var insuranceResponse = response.Content;
        insuranceResponse.Should().NotBeNull();
        insuranceResponse!.Number.Should().Be(insuranceRequest.Number);
    }

    [Fact]
    public async Task UpdateInsurance_InsuranceNotExist_ReturnsNotFound()
    {
        var insuranceRequest = InsuranceDataFaker.InsuranceRequestFaker
            .RuleFor(i => i.VehicleId, _ => Guid.NewGuid())
            .Generate();

        var response = await _insurancesApi.UpdateInsuranceAsync(Guid.NewGuid(), insuranceRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("Insurance.NotFound");
    }

    [Fact]
    public async Task DeleteInsurance_InsurancesExist_ReturnsNoContent()
    {
        var vehicle = await CreateVehicleAsync();
        var insurance = await CreateInsuranceAsync(vehicle);

        var response = await _insurancesApi.DeleteInsurancesAsync(insurance.Id);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteInsurance_InsuranceNotExist_ReturnsNotFound()
    {
        var response = await _insurancesApi.DeleteInsurancesAsync(Guid.NewGuid());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMessage = await response.Error!.GetContentAsAsync<ErrorMessage>();
        errorMessage.Should().NotBeNull();
        errorMessage!.Code.Should().Be("Insurance.NotFound");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();

    private async Task<Vehicle> CreateVehicleAsync()
    {
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        await AddEntityToDbAsync(vehicle);

        return vehicle;
    }

    private async Task<Insurance> CreateInsuranceAsync(Vehicle vehicle)
    {
        var insurance = InsuranceDataFaker.InsuranceFaker
            .RuleFor(i => i.VehicleId, _ => vehicle.Id)
            .Generate();

        await AddEntityToDbAsync(insurance);

        return insurance;
    }
}
