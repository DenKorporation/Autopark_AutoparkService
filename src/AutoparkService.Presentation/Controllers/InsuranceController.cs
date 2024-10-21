using Asp.Versioning;
using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.Insurance.Request;
using AutoparkService.Application.UseCases.Commands.Insurances.Create;
using AutoparkService.Application.UseCases.Commands.Insurances.Delete;
using AutoparkService.Application.UseCases.Commands.Insurances.Update;
using AutoparkService.Application.UseCases.Queries.Insurances.GetAll;
using AutoparkService.Application.UseCases.Queries.Insurances.GetById;
using AutoparkService.Domain.Constants;
using AutoparkService.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoparkService.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Administrator}, {Roles.FleetManager}, {Roles.InsuranceAgent}")]
[Route("api/v{apiVersion:apiVersion}/insurances")]
public class InsuranceController(ISender sender)
    : ControllerBase
{
    [HttpGet("")]
    public async Task<IResult> GetAllInsurancesAsync(
        [FromQuery] FilterInsurancesRequest filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAllInsurancesQuery(filterRequest), cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{insuranceId:guid}")]
    public async Task<IResult> GetInsuranceByIdAsync(Guid insuranceId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetInsuranceByIdQuery(insuranceId), cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    public async Task<IResult> CreateInsuranceAsync(
        [FromBody] InsuranceRequest createInsuranceRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CreateInsuranceCommand(createInsuranceRequest), cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{insuranceId:guid}")]
    public async Task<IResult> UpdateInsuranceAsync(
        Guid insuranceId,
        [FromBody] InsuranceRequest updateInsuranceRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new UpdateInsuranceCommand(insuranceId, updateInsuranceRequest),
            cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{insuranceId:guid}")]
    public async Task<IResult> DeleteInsurancesAsync(Guid insuranceId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new DeleteInsuranceCommand(insuranceId), cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
