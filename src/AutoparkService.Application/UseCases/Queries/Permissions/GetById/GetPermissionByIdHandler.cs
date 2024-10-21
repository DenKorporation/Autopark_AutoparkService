using AutoMapper;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Application.Errors.Permission;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Queries.Permissions.GetById;

public class GetPermissionByIdHandler(
    IMapper mapper,
    IPermissionRepository permissionRepository)
    : IQueryHandler<GetPermissionByIdQuery, PermissionResponse>
{
    public async Task<Result<PermissionResponse>> Handle(
        GetPermissionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var permission = await permissionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (permission is null)
        {
            return new PermissionNotFoundError(request.Id);
        }

        return mapper.Map<PermissionResponse>(permission);
    }
}
