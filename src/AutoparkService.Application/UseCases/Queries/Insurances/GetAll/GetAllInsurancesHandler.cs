using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.Domain.Specifications.Insurances;
using FluentResults;

namespace AutoparkService.Application.UseCases.Queries.Insurances.GetAll;

public class GetAllInsurancesHandler(
    IMapper mapper,
    IInsuranceRepository insuranceRepository)
    : IQueryHandler<GetAllInsurancesQuery, PagedList<InsuranceResponse>>
{
    public async Task<Result<PagedList<InsuranceResponse>>> Handle(
        GetAllInsurancesQuery request,
        CancellationToken cancellationToken)
    {
        var specification = CreateSpecification(request);

        var resultCollection = await insuranceRepository.GetAllAsync(
            request.Request.PageNumber,
            request.Request.PageSize,
            q => q.ProjectTo<InsuranceResponse>(mapper.ConfigurationProvider),
            specification,
            cancellationToken);

        var totalCount = await insuranceRepository.CountAsync(specification, cancellationToken);

        return new PagedList<InsuranceResponse>(
            resultCollection,
            request.Request.PageNumber,
            request.Request.PageSize,
            totalCount);
    }

    private static Specification<Insurance>? CreateSpecification(GetAllInsurancesQuery request)
    {
        Specification<Insurance>? specification = null;

        if (request.Request.Series is not null)
        {
            var seriesMatchToSpecification = new SeriesMatchToSpecification(request.Request.Series);

            if (specification is null)
            {
                specification = seriesMatchToSpecification;
            }
            else
            {
                specification.And(seriesMatchToSpecification);
            }
        }

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

        if (request.Request.VehicleType is not null)
        {
            var vehicleTypeMatchToSpecification = new VehicleTypeMatchToSpecification(request.Request.VehicleType);

            if (specification is null)
            {
                specification = vehicleTypeMatchToSpecification;
            }
            else
            {
                specification.And(vehicleTypeMatchToSpecification);
            }
        }

        if (request.Request.Provider is not null)
        {
            var providerMatchToSpecification = new ProviderMatchToSpecification(request.Request.Provider);

            if (specification is null)
            {
                specification = providerMatchToSpecification;
            }
            else
            {
                specification.And(providerMatchToSpecification);
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
