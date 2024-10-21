using AutoparkService.Application.DTOs.Vehicle.Request;
using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.Vehicles.Create;

public record CreateVehicleCommand(VehicleRequest Request) : ICommand<VehicleResponse>;
