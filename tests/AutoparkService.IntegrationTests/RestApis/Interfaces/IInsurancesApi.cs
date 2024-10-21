using AutoparkService.Application.DTOs.Insurance.Request;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Application.UseCases.Queries.Insurances.GetAll;
using AutoparkService.IntegrationTests.Responses;
using Refit;

namespace AutoparkService.IntegrationTests.RestApis.Interfaces;

public interface IInsurancesApi
{
    [Get("/insurances")]
    Task<ApiResponse<PagedList<InsuranceResponse>>> GetAllInsurancesAsync(
        [Query] FilterInsurancesRequest filterRequest,
        CancellationToken cancellationToken = default);

    [Get("/insurances/{insuranceId}")]
    Task<ApiResponse<InsuranceResponse>> GetInsuranceByIdAsync(
        Guid insuranceId,
        CancellationToken cancellationToken = default);

    [Post("/insurances")]
    Task<ApiResponse<InsuranceResponse>> CreateInsuranceAsync(
        [Body] InsuranceRequest createInsuranceRequest,
        CancellationToken cancellationToken = default);

    [Put("/insurances/{insuranceId}")]
    Task<ApiResponse<InsuranceResponse>> UpdateInsuranceAsync(
        Guid insuranceId,
        [Body] InsuranceRequest updateInsuranceRequest,
        CancellationToken cancellationToken = default);

    [Delete("/insurances/{insuranceId}")]
    Task<ApiResponse<string>> DeleteInsurancesAsync(
        Guid insuranceId,
        CancellationToken cancellationToken = default);
}
