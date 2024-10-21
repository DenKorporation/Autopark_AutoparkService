using AutoMapper;
using AutoparkService.Application.DTOs.Permission.Request;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Permission;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.Permissions.Create;

public class CreatePermissionHandler(
    IPermissionRepository permissionRepository,
    IVehicleRepository vehicleRepository,
    IMapper mapper)
    : ICommandHandler<CreatePermissionCommand, PermissionResponse>
{
    public async Task<Result<PermissionResponse>> Handle(
        CreatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request.Request, cancellationToken);

        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        var createPermission = mapper.Map<Permission>(request.Request);

        try
        {
            await permissionRepository.CreateAsync(createPermission, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("Permission.Create");
        }

        return mapper.Map<PermissionResponse>(createPermission);
    }

    private async Task<Result> ValidateRequest(PermissionRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);

        if (vehicle is null)
        {
            return new VehicleNotFoundError(request.VehicleId);
        }

        var permissionForVehicle =
            await permissionRepository.GetByVehicleId(request.VehicleId, cancellationToken);

        if (permissionForVehicle is not null)
        {
            return new PermissionDuplicationError(request.VehicleId);
        }

        var permission = await permissionRepository.GetByNumberAsync(request.Number, cancellationToken);

        if (permission is not null)
        {
            return new PermissionDuplicationError(request.Number);
        }

        return Result.Ok();
    }
}
