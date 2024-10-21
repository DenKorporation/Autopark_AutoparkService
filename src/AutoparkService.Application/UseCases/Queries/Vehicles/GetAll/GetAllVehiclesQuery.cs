using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Queries.Vehicles.GetAll;

public record GetAllVehiclesQuery(FilterVehiclesRequest Request)
    : IQuery<PagedList<VehicleResponse>>;
