using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.Vehicles.Delete;

public class DeleteVehicleHandler(
    IVehicleRepository vehicleRepository)
    : ICommandHandler<DeleteVehicleCommand>
{
    public async Task<Result> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (vehicle is null)
        {
            return new VehicleNotFoundError(request.Id);
        }

        try
        {
            await vehicleRepository.DeleteAsync(vehicle, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("Vehicle.Delete");
        }

        return Result.Ok();
    }
}
