using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.Permissions.Delete;

public record DeletePermissionCommand(Guid Id) : ICommand;
