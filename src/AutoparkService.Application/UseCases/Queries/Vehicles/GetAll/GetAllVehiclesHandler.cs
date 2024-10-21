using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.Domain.Specifications.Vehicles;
using FluentResults;

namespace AutoparkService.Application.UseCases.Queries.Vehicles.GetAll;

public class GetAllVehiclesHandler(
    IMapper mapper,
    IVehicleRepository vehicleRepository)
    : IQueryHandler<GetAllVehiclesQuery, PagedList<VehicleResponse>>
{
    public async Task<Result<PagedList<VehicleResponse>>> Handle(
        GetAllVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        var specification = CreateSpecification(request);

        var resultCollection = await vehicleRepository.GetAllAsync(
            request.Request.PageNumber,
            request.Request.PageSize,
            q => q.ProjectTo<VehicleResponse>(mapper.ConfigurationProvider),
            specification,
            cancellationToken);

        var totalCount = await vehicleRepository.CountAsync(specification, cancellationToken);

        return new PagedList<VehicleResponse>(
            resultCollection,
            request.Request.PageNumber,
            request.Request.PageSize,
            totalCount);
    }

    private static Specification<Vehicle>? CreateSpecification(GetAllVehiclesQuery request)
    {
        Specification<Vehicle>? specification = null;

        if (request.Request.OdometerFrom.HasValue)
        {
            var odometerAtLeastSpecification = new OdometerAtLeastSpecification(request.Request.OdometerFrom.Value);

            if (specification is null)
            {
                specification = odometerAtLeastSpecification;
            }
            else
            {
                specification.And(odometerAtLeastSpecification);
            }
        }

        if (request.Request.OdometerTo.HasValue)
        {
            var odometerAtMostSpecification = new OdometerAtMostSpecification(request.Request.OdometerTo.Value);

            if (specification is null)
            {
                specification = odometerAtMostSpecification;
            }
            else
            {
                specification.And(odometerAtMostSpecification);
            }
        }

        if (request.Request.PurchaseDateFrom is not null)
        {
            var purchaseDateAtLeastSpecification =
                new PurchaseDateAtLeastSpecification(DateOnly.Parse(request.Request.PurchaseDateFrom));

            if (specification is null)
            {
                specification = purchaseDateAtLeastSpecification;
            }
            else
            {
                specification.And(purchaseDateAtLeastSpecification);
            }
        }

        if (request.Request.PurchaseDateTo is not null)
        {
            var purchaseDateAtMostSpecification =
                new PurchaseDateAtMostSpecification(DateOnly.Parse(request.Request.PurchaseDateTo));

            if (specification is null)
            {
                specification = purchaseDateAtMostSpecification;
            }
            else
            {
                specification.And(purchaseDateAtMostSpecification);
            }
        }

        if (request.Request.CostFrom.HasValue)
        {
            var costAtLeastSpecification = new CostAtLeastSpecification(request.Request.CostFrom.Value);

            if (specification is null)
            {
                specification = costAtLeastSpecification;
            }
            else
            {
                specification.And(costAtLeastSpecification);
            }
        }

        if (request.Request.CostTo.HasValue)
        {
            var odometerAtMostSpecification = new CostAtMostSpecification(request.Request.CostTo.Value);

            if (specification is null)
            {
                specification = odometerAtMostSpecification;
            }
            else
            {
                specification.And(odometerAtMostSpecification);
            }
        }

        if (request.Request.Status is not null)
        {
            var statusEqualToSpecification = new StatusEqualToSpecification(request.Request.Status);

            if (specification is null)
            {
                specification = statusEqualToSpecification;
            }
            else
            {
                specification.And(statusEqualToSpecification);
            }
        }

        if (request.Request.UserId is not null)
        {
            var userIdEqualToSpecification = new UserIdEqualToSpecification(request.Request.UserId.Value);

            if (specification is null)
            {
                specification = userIdEqualToSpecification;
            }
            else
            {
                specification.And(userIdEqualToSpecification);
            }
        }

        return specification;
    }
}
