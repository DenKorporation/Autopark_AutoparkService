using Asp.Versioning;
using AutoparkService.Application.DTOs.Vehicle.Request;
using AutoparkService.Application.UseCases.Commands.Vehicles.Create;
using AutoparkService.Application.UseCases.Commands.Vehicles.Delete;
using AutoparkService.Application.UseCases.Commands.Vehicles.Update;
using AutoparkService.Application.UseCases.Queries.Vehicles.GetAll;
using AutoparkService.Application.UseCases.Queries.Vehicles.GetById;
using AutoparkService.Domain.Constants;
using AutoparkService.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoparkService.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Administrator}, {Roles.FleetManager}, {Roles.InsuranceAgent}, {Roles.Driver}")]
[Route("api/v{apiVersion:apiVersion}/vehicles")]
public class VehicleController(ISender sender)
    : ControllerBase
{
    [HttpGet("")]
    public async Task<IResult> GetAllVehiclesAsync(
        [FromQuery] FilterVehiclesRequest filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAllVehiclesQuery(filterRequest), cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{vehicleId:guid}")]
    public async Task<IResult> GetVehicleByIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetVehicleByIdQuery(vehicleId), cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    public async Task<IResult> CreateVehicleAsync(
        [FromBody] VehicleRequest createVehicleRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CreateVehicleCommand(createVehicleRequest), cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{vehicleId:guid}")]
    public async Task<IResult> UpdateVehicleAsync(
        Guid vehicleId,
        [FromBody] VehicleRequest updateVehicleRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new UpdateVehicleCommand(vehicleId, updateVehicleRequest),
            cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{vehicleId:guid}")]
    public async Task<IResult> DeleteVehiclesAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new DeleteVehicleCommand(vehicleId), cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
