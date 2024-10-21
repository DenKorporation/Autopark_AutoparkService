using AutoparkService.Application.DTOs.TechnicalPassport.Request;
using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetAll;
using AutoparkService.IntegrationTests.Responses;
using Refit;

namespace AutoparkService.IntegrationTests.RestApis.Interfaces;

public interface ITechnicalPassportsApi
{
    [Get("/technicalPassports")]
    Task<ApiResponse<PagedList<TechnicalPassportResponse>>> GetAllTechnicalPassportsAsync(
        [Query] FilterTechnicalPassportsRequest filterRequest,
        CancellationToken cancellationToken = default);

    [Get("/technicalPassports/{technicalPassportId}")]
    Task<ApiResponse<TechnicalPassportResponse>> GetTechnicalPassportByIdAsync(
        Guid technicalPassportId,
        CancellationToken cancellationToken = default);

    [Post("/technicalPassports")]
    Task<ApiResponse<TechnicalPassportResponse>> CreateTechnicalPassportAsync(
        [Body] TechnicalPassportRequest createTechnicalPassportRequest,
        CancellationToken cancellationToken = default);

    [Put("/technicalPassports/{technicalPassportId}")]
    Task<ApiResponse<TechnicalPassportResponse>> UpdateTechnicalPassportAsync(
        Guid technicalPassportId,
        [Body] TechnicalPassportRequest updateTechnicalPassportRequest,
        CancellationToken cancellationToken = default);

    [Delete("/technicalPassports/{technicalPassportId}")]
    Task<ApiResponse<string>> DeleteTechnicalPassportsAsync(
        Guid technicalPassportId,
        CancellationToken cancellationToken = default);
}
