using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.Domain.Specifications.MaintenanceRecords;
using FluentResults;

namespace AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetAll;

public class GetAllMaintenanceRecordsHandler(
    IMapper mapper,
    IMaintenanceRecordRepository maintenanceRecordRepository)
    : IQueryHandler<GetAllMaintenanceRecordsQuery, PagedList<MaintenanceRecordResponse>>
{
    public async Task<Result<PagedList<MaintenanceRecordResponse>>> Handle(
        GetAllMaintenanceRecordsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = CreateSpecification(request);

        var resultCollection = await maintenanceRecordRepository.GetAllAsync(
            request.Request.PageNumber,
            request.Request.PageSize,
            q => q.ProjectTo<MaintenanceRecordResponse>(mapper.ConfigurationProvider),
            specification,
            cancellationToken);

        var totalCount = await maintenanceRecordRepository.CountAsync(specification, cancellationToken);

        return new PagedList<MaintenanceRecordResponse>(
            resultCollection,
            request.Request.PageNumber,
            request.Request.PageSize,
            totalCount);
    }

    private static Specification<MaintenanceRecord>? CreateSpecification(GetAllMaintenanceRecordsQuery request)
    {
        Specification<MaintenanceRecord>? specification = null;

        if (request.Request.Type is not null)
        {
            var typeMatchToSpecification = new TypeMatchToSpecification(request.Request.Type);

            if (specification is null)
            {
                specification = typeMatchToSpecification;
            }
            else
            {
                specification.And(typeMatchToSpecification);
            }
        }

        if (request.Request.StartDate is not null)
        {
            var startDateAtLeastSpecification =
                new StartDateAtLeastSpecification(DateOnly.Parse(request.Request.StartDate));

            if (specification is null)
            {
                specification = startDateAtLeastSpecification;
            }
            else
            {
                specification.And(startDateAtLeastSpecification);
            }
        }

        if (request.Request.EndDate is not null)
        {
            var endDateAtMostSpecification =
                new EndDateAtMostSpecification(DateOnly.Parse(request.Request.EndDate));

            if (specification is null)
            {
                specification = endDateAtMostSpecification;
            }
            else
            {
                specification.And(endDateAtMostSpecification);
            }
        }

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

        if (request.Request.ServiceCenter is not null)
        {
            var serviceCenterMatchToSpecification =
                new ServiceCenterMatchToSpecification(request.Request.ServiceCenter);

            if (specification is null)
            {
                specification = serviceCenterMatchToSpecification;
            }
            else
            {
                specification.And(serviceCenterMatchToSpecification);
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
            var costAtMostSpecification = new CostAtMostSpecification(request.Request.CostTo.Value);

            if (specification is null)
            {
                specification = costAtMostSpecification;
            }
            else
            {
                specification.And(costAtMostSpecification);
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

        return specification;
    }
}
