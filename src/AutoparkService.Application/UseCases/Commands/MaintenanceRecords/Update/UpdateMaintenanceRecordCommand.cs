using AutoparkService.Application.DTOs.MaintenanceRecord.Request;
using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Update;

public record UpdateMaintenanceRecordCommand(Guid Id, MaintenanceRecordRequest Request)
    : ICommand<MaintenanceRecordResponse>;
