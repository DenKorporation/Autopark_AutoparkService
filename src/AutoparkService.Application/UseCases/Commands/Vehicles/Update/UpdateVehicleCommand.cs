using AutoparkService.Application.DTOs.Vehicle.Request;
using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.Vehicles.Update;

public record UpdateVehicleCommand(Guid Id, VehicleRequest Request)
    : ICommand<VehicleResponse>;
