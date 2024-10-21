using Asp.Versioning;
using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.MaintenanceRecord.Request;
using AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Create;
using AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Delete;
using AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Update;
using AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetAll;
using AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetById;
using AutoparkService.Domain.Constants;
using AutoparkService.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoparkService.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Administrator}, {Roles.FleetManager}, {Roles.Technician}")]
[Route("api/v{apiVersion:apiVersion}/maintenanceRecords")]
public class MaintenanceRecordController(ISender sender)
    : ControllerBase
{
    [HttpGet("")]
    public async Task<IResult> GetAllMaintenanceRecordsAsync(
        [FromQuery] FilterMaintenanceRecordsRequest filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAllMaintenanceRecordsQuery(filterRequest), cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{maintenanceRecordId:guid}")]
    public async Task<IResult> GetMaintenanceRecordByIdAsync(Guid maintenanceRecordId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetMaintenanceRecordByIdQuery(maintenanceRecordId), cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    public async Task<IResult> CreateMaintenanceRecordAsync(
        [FromBody] MaintenanceRecordRequest createMaintenanceRecordRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CreateMaintenanceRecordCommand(createMaintenanceRecordRequest), cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{maintenanceRecordId:guid}")]
    public async Task<IResult> UpdateMaintenanceRecordAsync(
        Guid maintenanceRecordId,
        [FromBody] MaintenanceRecordRequest updateMaintenanceRecordRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new UpdateMaintenanceRecordCommand(maintenanceRecordId, updateMaintenanceRecordRequest),
            cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{maintenanceRecordId:guid}")]
    public async Task<IResult> DeleteMaintenanceRecordsAsync(Guid maintenanceRecordId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new DeleteMaintenanceRecordCommand(maintenanceRecordId), cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
