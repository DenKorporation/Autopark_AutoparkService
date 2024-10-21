using AutoMapper;
using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Queries.Vehicles.GetById;

public class GetVehicleByIdHandler(
    IMapper mapper,
    IVehicleRepository vehicleRepository)
    : IQueryHandler<GetVehicleByIdQuery, VehicleResponse>
{
    public async Task<Result<VehicleResponse>> Handle(
        GetVehicleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (vehicle is null)
        {
            return new VehicleNotFoundError(request.Id);
        }

        await vehicleRepository.LoadRelatedData(vehicle, cancellationToken);

        return mapper.Map<VehicleResponse>(vehicle);
    }
}
