using AutoparkService.Application.DTOs.MaintenanceRecord.Request;
using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetAll;
using AutoparkService.IntegrationTests.Responses;
using Refit;

namespace AutoparkService.IntegrationTests.RestApis.Interfaces;

public interface IMaintenanceRecordsApi
{
    [Get("/maintenanceRecords")]
    Task<ApiResponse<PagedList<MaintenanceRecordResponse>>> GetAllMaintenanceRecordsAsync(
        [Query] FilterMaintenanceRecordsRequest filterRequest,
        CancellationToken cancellationToken = default);

    [Get("/maintenanceRecords/{maintenanceRecordId}")]
    Task<ApiResponse<MaintenanceRecordResponse>> GetMaintenanceRecordByIdAsync(
        Guid maintenanceRecordId,
        CancellationToken cancellationToken = default);

    [Post("/maintenanceRecords")]
    Task<ApiResponse<MaintenanceRecordResponse>> CreateMaintenanceRecordAsync(
        [Body] MaintenanceRecordRequest createMaintenanceRecordRequest,
        CancellationToken cancellationToken = default);

    [Put("/maintenanceRecords/{maintenanceRecordId}")]
    Task<ApiResponse<MaintenanceRecordResponse>> UpdateMaintenanceRecordAsync(
        Guid maintenanceRecordId,
        [Body] MaintenanceRecordRequest updateMaintenanceRecordRequest,
        CancellationToken cancellationToken = default);

    [Delete("/maintenanceRecords/{maintenanceRecordId}")]
    Task<ApiResponse<string>> DeleteMaintenanceRecordsAsync(
        Guid maintenanceRecordId,
        CancellationToken cancellationToken = default);
}
