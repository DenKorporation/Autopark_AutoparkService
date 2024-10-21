using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.Vehicles.Delete;

public record DeleteVehicleCommand(Guid Id) : ICommand;
