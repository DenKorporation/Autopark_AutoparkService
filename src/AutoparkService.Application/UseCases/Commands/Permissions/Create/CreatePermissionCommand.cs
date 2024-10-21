using AutoparkService.Application.DTOs.Permission.Request;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.Permissions.Create;

public record CreatePermissionCommand(PermissionRequest Request)
    : ICommand<PermissionResponse>;
