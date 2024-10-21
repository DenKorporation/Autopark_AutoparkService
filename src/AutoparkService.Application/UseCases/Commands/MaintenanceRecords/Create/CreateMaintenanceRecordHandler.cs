using AutoMapper;
using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Create;

public class CreateMaintenanceRecordHandler(
    IMaintenanceRecordRepository maintenanceRecordRepository,
    IVehicleRepository vehicleRepository,
    IMapper mapper)
    : ICommandHandler<CreateMaintenanceRecordCommand, MaintenanceRecordResponse>
{
    public async Task<Result<MaintenanceRecordResponse>> Handle(
        CreateMaintenanceRecordCommand request,
        CancellationToken cancellationToken)
    {
        var createDto = request.Request;

        var vehicle = await vehicleRepository.GetByIdAsync(createDto.VehicleId, cancellationToken);

        if (vehicle is null)
        {
            return new VehicleNotFoundError(createDto.VehicleId);
        }

        var createMaintenanceRecord = mapper.Map<MaintenanceRecord>(createDto);

        try
        {
            await maintenanceRecordRepository.CreateAsync(createMaintenanceRecord, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("MaintenanceRecord.Create");
        }

        return mapper.Map<MaintenanceRecordResponse>(createMaintenanceRecord);
    }
}
