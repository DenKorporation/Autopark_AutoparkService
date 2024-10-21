using Asp.Versioning;
using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.Permission.Request;
using AutoparkService.Application.UseCases.Commands.Permissions.Create;
using AutoparkService.Application.UseCases.Commands.Permissions.Delete;
using AutoparkService.Application.UseCases.Commands.Permissions.Update;
using AutoparkService.Application.UseCases.Queries.Permissions.GetAll;
using AutoparkService.Application.UseCases.Queries.Permissions.GetById;
using AutoparkService.Domain.Constants;
using AutoparkService.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoparkService.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Administrator}, {Roles.FleetManager}, {Roles.Technician}")]
[Route("api/v{apiVersion:apiVersion}/permissions")]
public class PermissionController(ISender sender)
    : ControllerBase
{
    [HttpGet("")]
    public async Task<IResult> GetAllPermissionsAsync(
        [FromQuery] FilterPermissionsRequest filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAllPermissionsQuery(filterRequest), cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{permissionId:guid}")]
    public async Task<IResult> GetPermissionByIdAsync(Guid permissionId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetPermissionByIdQuery(permissionId), cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    public async Task<IResult> CreatePermissionAsync(
        [FromBody] PermissionRequest createPermissionRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CreatePermissionCommand(createPermissionRequest), cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{permissionId:guid}")]
    public async Task<IResult> UpdatePermissionAsync(
        Guid permissionId,
        [FromBody] PermissionRequest updatePermissionRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new UpdatePermissionCommand(permissionId, updatePermissionRequest),
            cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{permissionId:guid}")]
    public async Task<IResult> DeletePermissionsAsync(Guid permissionId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new DeletePermissionCommand(permissionId), cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
