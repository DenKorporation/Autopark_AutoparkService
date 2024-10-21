using AutoMapper;
using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.Vehicles.Update;

public class UpdateVehicleHandler(
    IVehicleRepository vehicleRepository,
    IMapper mapper)
    : ICommandHandler<UpdateVehicleCommand, VehicleResponse>
{
    public async Task<Result<VehicleResponse>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var updateDto = request.Request;

        var vehicle = await vehicleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (vehicle is null)
        {
            return new VehicleNotFoundError(request.Id);
        }

        mapper.Map(updateDto, vehicle);

        try
        {
            await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("Vehicle.Update");
        }

        return mapper.Map<VehicleResponse>(vehicle);
    }
}
