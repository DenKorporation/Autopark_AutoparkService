using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.MaintenanceRecord;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Delete;

public class DeleteMaintenanceRecordHandler(IMaintenanceRecordRepository maintenanceRecordRepository)
    : ICommandHandler<DeleteMaintenanceRecordCommand>
{
    public async Task<Result> Handle(DeleteMaintenanceRecordCommand request, CancellationToken cancellationToken)
    {
        var maintenanceRecord = await maintenanceRecordRepository.GetByIdAsync(request.Id, cancellationToken);

        if (maintenanceRecord is null)
        {
            return new MaintenanceRecordNotFoundError(request.Id);
        }

        try
        {
            await maintenanceRecordRepository.DeleteAsync(maintenanceRecord, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("MaintenanceRecord.Delete");
        }

        return Result.Ok();
    }
}
