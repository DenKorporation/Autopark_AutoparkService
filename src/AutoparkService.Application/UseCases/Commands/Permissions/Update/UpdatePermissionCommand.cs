using AutoparkService.Application.DTOs.Permission.Request;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.Permissions.Update;

public record UpdatePermissionCommand(Guid Id, PermissionRequest Request) : ICommand<PermissionResponse>;
