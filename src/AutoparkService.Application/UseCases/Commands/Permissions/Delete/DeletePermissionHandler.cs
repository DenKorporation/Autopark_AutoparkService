using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Permission;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.Permissions.Delete;

public class DeletePermissionHandler(
    IPermissionRepository permissionRepository)
    : ICommandHandler<DeletePermissionCommand>
{
    public async Task<Result> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await permissionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (permission is null)
        {
            return new PermissionNotFoundError(request.Id);
        }

        try
        {
            await permissionRepository.DeleteAsync(permission, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("Permission.Delete");
        }

        return Result.Ok();
    }
}
