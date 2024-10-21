using AutoMapper;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Permission;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.Permissions.Update;

public class UpdatePermissionHandler(
    IPermissionRepository permissionRepository,
    IVehicleRepository vehicleRepository,
    IMapper mapper)
    : ICommandHandler<UpdatePermissionCommand, PermissionResponse>
{
    public async Task<Result<PermissionResponse>> Handle(
        UpdatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);

        if (validationResult.IsFailed)
        {
            return validationResult.ToResult();
        }

        var permission = validationResult.Value;

        mapper.Map(request.Request, permission);

        try
        {
            await permissionRepository.UpdateAsync(permission, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("Permission.Update");
        }

        return mapper.Map<PermissionResponse>(permission);
    }

    private async Task<Result<Permission>> ValidateRequest(
        UpdatePermissionCommand request,
        CancellationToken cancellationToken = default)
    {
        var updateDto = request.Request;

        var permission = await permissionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (permission is null)
        {
            return new PermissionNotFoundError(request.Id);
        }

        if (permission.Number != updateDto.Number)
        {
            var duplicatePermission = await permissionRepository.GetByNumberAsync(updateDto.Number, cancellationToken);

            if (duplicatePermission is not null)
            {
                return new PermissionDuplicationError(updateDto.Number);
            }
        }

        if (permission.VehicleId != updateDto.VehicleId)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(updateDto.VehicleId, cancellationToken);

            if (vehicle is null)
            {
                return new VehicleNotFoundError(updateDto.VehicleId);
            }

            var permissionForVehicle =
                await permissionRepository.GetByVehicleId(updateDto.VehicleId, cancellationToken);

            if (permissionForVehicle is not null)
            {
                return new PermissionDuplicationError(updateDto.VehicleId);
            }
        }

        return permission;
    }
}
