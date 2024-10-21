using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.Domain.Specifications.Permissions;
using FluentResults;

namespace AutoparkService.Application.UseCases.Queries.Permissions.GetAll;

public class GetAllPermissionsHandler(
    IMapper mapper,
    IPermissionRepository permissionRepository)
    : IQueryHandler<GetAllPermissionsQuery, PagedList<PermissionResponse>>
{
    public async Task<Result<PagedList<PermissionResponse>>> Handle(
        GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = CreateSpecification(request);

        var resultCollection = await permissionRepository.GetAllAsync(
            request.Request.PageNumber,
            request.Request.PageSize,
            q => q.ProjectTo<PermissionResponse>(mapper.ConfigurationProvider),
            specification,
            cancellationToken);

        var totalCount = await permissionRepository.CountAsync(specification, cancellationToken);

        return new PagedList<PermissionResponse>(
            resultCollection,
            request.Request.PageNumber,
            request.Request.PageSize,
            totalCount);
    }

    private static Specification<Permission>? CreateSpecification(GetAllPermissionsQuery request)
    {
        Specification<Permission>? specification = null;

        if (request.Request.Number is not null)
        {
            var numberMatchToSpecification = new NumberMatchToSpecification(request.Request.Number);

            if (specification is null)
            {
                specification = numberMatchToSpecification;
            }
            else
            {
                specification.And(numberMatchToSpecification);
            }
        }

        if (request.Request.ExpiryDateFrom is not null)
        {
            var expiryDateAtLeastSpecification =
                new ExpiryDateAtLeastSpecification(DateOnly.Parse(request.Request.ExpiryDateFrom));

            if (specification is null)
            {
                specification = expiryDateAtLeastSpecification;
            }
            else
            {
                specification.And(expiryDateAtLeastSpecification);
            }
        }

        if (request.Request.ExpiryDateTo is not null)
        {
            var expiryDateAtMostSpecification =
                new ExpiryDateAtMostSpecification(DateOnly.Parse(request.Request.ExpiryDateTo));

            if (specification is null)
            {
                specification = expiryDateAtMostSpecification;
            }
            else
            {
                specification.And(expiryDateAtMostSpecification);
            }
        }

        if (request.Request.VehicleId is not null)
        {
            var vehicleIdEqualToSpecification = new VehicleIdEqualToSpecification(request.Request.VehicleId.Value);

            if (specification is null)
            {
                specification = vehicleIdEqualToSpecification;
            }
            else
            {
                specification.And(vehicleIdEqualToSpecification);
            }
        }

        if (request.Request.IsValid.HasValue)
        {
            var isValidSpecification = new IsValidSpecification(request.Request.IsValid.Value);

            if (specification is null)
            {
                specification = isValidSpecification;
            }
            else
            {
                specification.And(isValidSpecification);
            }
        }

        return specification;
    }
}
