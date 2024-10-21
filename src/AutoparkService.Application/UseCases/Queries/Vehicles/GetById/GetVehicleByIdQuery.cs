using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Queries.Vehicles.GetById;

public record GetVehicleByIdQuery(Guid Id) : IQuery<VehicleResponse>;
