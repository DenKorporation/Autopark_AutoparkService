using AutoparkService.Application.DTOs.MaintenanceRecord.Request;
using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Create;

public record CreateMaintenanceRecordCommand(MaintenanceRecordRequest Request)
    : ICommand<MaintenanceRecordResponse>;
