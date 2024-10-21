using AutoparkService.Application.DTOs.Permission.Request;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Application.UseCases.Queries.Permissions.GetAll;
using AutoparkService.IntegrationTests.Responses;
using Refit;

namespace AutoparkService.IntegrationTests.RestApis.Interfaces;

public interface IPermissionsApi
{
    [Get("/permissions")]
    Task<ApiResponse<PagedList<PermissionResponse>>> GetAllPermissionsAsync(
        [Query] FilterPermissionsRequest filterRequest,
        CancellationToken cancellationToken = default);

    [Get("/permissions/{permissionId}")]
    Task<ApiResponse<PermissionResponse>> GetPermissionByIdAsync(
        Guid permissionId,
        CancellationToken cancellationToken = default);

    [Post("/permissions")]
    Task<ApiResponse<PermissionResponse>> CreatePermissionAsync(
        [Body] PermissionRequest createPermissionRequest,
        CancellationToken cancellationToken = default);

    [Put("/permissions/{permissionId}")]
    Task<ApiResponse<PermissionResponse>> UpdatePermissionAsync(
        Guid permissionId,
        [Body] PermissionRequest updatePermissionRequest,
        CancellationToken cancellationToken = default);

    [Delete("/permissions/{permissionId}")]
    Task<ApiResponse<string>> DeletePermissionsAsync(
        Guid permissionId,
        CancellationToken cancellationToken = default);
}
