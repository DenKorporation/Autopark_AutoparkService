using AutoMapper;
using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Application.Errors.MaintenanceRecord;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetById;

public class GetMaintenanceRecordByIdHandler(
    IMapper mapper,
    IMaintenanceRecordRepository maintenanceRecordRepository)
    : IQueryHandler<GetMaintenanceRecordByIdQuery, MaintenanceRecordResponse>
{
    public async Task<Result<MaintenanceRecordResponse>> Handle(
        GetMaintenanceRecordByIdQuery request,
        CancellationToken cancellationToken)
    {
        var maintenanceRecord = await maintenanceRecordRepository.GetByIdAsync(request.Id, cancellationToken);

        if (maintenanceRecord is null)
        {
            return new MaintenanceRecordNotFoundError(request.Id);
        }

        return mapper.Map<MaintenanceRecordResponse>(maintenanceRecord);
    }
}
