using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Queries.Permissions.GetById;

public record GetPermissionByIdQuery(Guid Id) : IQuery<PermissionResponse>;
