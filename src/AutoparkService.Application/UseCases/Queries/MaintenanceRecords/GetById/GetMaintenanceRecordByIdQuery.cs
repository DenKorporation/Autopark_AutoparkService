using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetById;

public record GetMaintenanceRecordByIdQuery(Guid Id)
    : IQuery<MaintenanceRecordResponse>;
