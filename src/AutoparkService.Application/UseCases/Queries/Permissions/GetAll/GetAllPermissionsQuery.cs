using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Queries.Permissions.GetAll;

public record GetAllPermissionsQuery(FilterPermissionsRequest Request)
    : IQuery<PagedList<PermissionResponse>>;
