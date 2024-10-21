using Asp.Versioning;
using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.TechnicalPassport.Request;
using AutoparkService.Application.UseCases.Commands.TechnicalPassports.Create;
using AutoparkService.Application.UseCases.Commands.TechnicalPassports.Delete;
using AutoparkService.Application.UseCases.Commands.TechnicalPassports.Update;
using AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetAll;
using AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetById;
using AutoparkService.Domain.Constants;
using AutoparkService.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoparkService.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Administrator}, {Roles.FleetManager}, {Roles.Technician}")]
[Route("api/v{apiVersion:apiVersion}/technicalPassports")]
public class TechnicalPassportController(ISender sender)
    : ControllerBase
{
    [HttpGet("")]
    public async Task<IResult> GetAllTechnicalPassportsAsync(
        [FromQuery] FilterTechnicalPassportsRequest filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAllTechnicalPassportsQuery(filterRequest), cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{technicalPassportId:guid}")]
    public async Task<IResult> GetTechnicalPassportByIdAsync(Guid technicalPassportId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetTechnicalPassportByIdQuery(technicalPassportId), cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    public async Task<IResult> CreateTechnicalPassportAsync(
        [FromBody] TechnicalPassportRequest createTechnicalPassportRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CreateTechnicalPassportCommand(createTechnicalPassportRequest), cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{technicalPassportId:guid}")]
    public async Task<IResult> UpdateTechnicalPassportAsync(
        Guid technicalPassportId,
        [FromBody] TechnicalPassportRequest updateTechnicalPassportRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new UpdateTechnicalPassportCommand(technicalPassportId, updateTechnicalPassportRequest),
            cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{technicalPassportId:guid}")]
    public async Task<IResult> DeleteTechnicalPassportsAsync(Guid technicalPassportId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new DeleteTechnicalPassportCommand(technicalPassportId), cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
