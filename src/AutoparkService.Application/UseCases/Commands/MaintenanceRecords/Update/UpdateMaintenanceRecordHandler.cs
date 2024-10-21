using AutoMapper;
using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.MaintenanceRecord;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Update;

public class UpdateMaintenanceRecordHandler(
    IMaintenanceRecordRepository maintenanceRecordRepository,
    IVehicleRepository vehicleRepository,
    IMapper mapper)
    : ICommandHandler<UpdateMaintenanceRecordCommand, MaintenanceRecordResponse>
{
    public async Task<Result<MaintenanceRecordResponse>> Handle(
        UpdateMaintenanceRecordCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);

        if (validationResult.IsFailed)
        {
            return validationResult.ToResult();
        }

        var maintenanceRecord = validationResult.Value;

        mapper.Map(request.Request, maintenanceRecord);

        try
        {
            await maintenanceRecordRepository.UpdateAsync(maintenanceRecord, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("MaintenanceRecord.Update");
        }

        return mapper.Map<MaintenanceRecordResponse>(maintenanceRecord);
    }

    private async Task<Result<MaintenanceRecord>> ValidateRequest(
        UpdateMaintenanceRecordCommand request,
        CancellationToken cancellationToken = default)
    {
        var updateDto = request.Request;

        var maintenanceRecord = await maintenanceRecordRepository.GetByIdAsync(request.Id, cancellationToken);

        if (maintenanceRecord is null)
        {
            return new MaintenanceRecordNotFoundError(request.Id);
        }

        if (maintenanceRecord.VehicleId != updateDto.VehicleId)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(updateDto.VehicleId, cancellationToken);

            if (vehicle is null)
            {
                return new VehicleNotFoundError(updateDto.VehicleId);
            }
        }

        return maintenanceRecord;
    }
}
