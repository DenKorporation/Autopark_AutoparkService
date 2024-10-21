using AutoMapper;
using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.Vehicles.Create;

public class CreateVehicleHandler(
    IVehicleRepository vehicleRepository,
    IMapper mapper)
    : ICommandHandler<CreateVehicleCommand, VehicleResponse>
{
    public async Task<Result<VehicleResponse>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var createDto = request.Request;
        var createVehicle = mapper.Map<Vehicle>(createDto);

        try
        {
            await vehicleRepository.CreateAsync(createVehicle, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("Vehicle.Create");
        }

        return mapper.Map<VehicleResponse>(createVehicle);
    }
}
