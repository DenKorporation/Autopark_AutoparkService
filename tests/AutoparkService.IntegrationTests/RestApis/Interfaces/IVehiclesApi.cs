using AutoparkService.Application.DTOs.Vehicle.Request;
using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Application.UseCases.Queries.Vehicles.GetAll;
using AutoparkService.IntegrationTests.Responses;
using Refit;

namespace AutoparkService.IntegrationTests.RestApis.Interfaces;

public interface IVehiclesApi
{
    [Get("/vehicles")]
    Task<ApiResponse<PagedList<VehicleResponse>>> GetAllVehiclesAsync(
        [Query] FilterVehiclesRequest filterRequest,
        CancellationToken cancellationToken = default);

    [Get("/vehicles/{vehicleId}")]
    Task<ApiResponse<VehicleResponse>> GetVehicleByIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default);

    [Post("/vehicles")]
    Task<ApiResponse<VehicleResponse>> CreateVehicleAsync(
        [Body] VehicleRequest createVehicleRequest,
        CancellationToken cancellationToken = default);

    [Put("/vehicles/{vehicleId}")]
    Task<ApiResponse<VehicleResponse>> UpdateVehicleAsync(
        Guid vehicleId,
        [Body] VehicleRequest updateVehicleRequest,
        CancellationToken cancellationToken = default);

    [Delete("/vehicles/{vehicleId}")]
    Task<ApiResponse<string>> DeleteVehiclesAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default);
}
