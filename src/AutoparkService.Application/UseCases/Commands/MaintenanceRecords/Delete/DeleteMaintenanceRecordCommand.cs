using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Delete;

public record DeleteMaintenanceRecordCommand(Guid Id)
    : ICommand;
