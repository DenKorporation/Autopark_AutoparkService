using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.Domain.Specifications.TechnicalPassports;
using FluentResults;

namespace AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetAll;

public class GetAllTechnicalPassportsHandler(
    IMapper mapper,
    ITechnicalPassportRepository technicalPassportRepository)
    : IQueryHandler<GetAllTechnicalPassportsQuery, PagedList<TechnicalPassportResponse>>
{
    public async Task<Result<PagedList<TechnicalPassportResponse>>> Handle(
        GetAllTechnicalPassportsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = CreateSpecification(request);

        var resultCollection = await technicalPassportRepository.GetAllAsync(
            request.Request.PageNumber,
            request.Request.PageSize,
            q => q.ProjectTo<TechnicalPassportResponse>(mapper.ConfigurationProvider),
            specification,
            cancellationToken);

        var totalCount = await technicalPassportRepository.CountAsync(specification, cancellationToken);

        return new PagedList<TechnicalPassportResponse>(
            resultCollection,
            request.Request.PageNumber,
            request.Request.PageSize,
            totalCount);
    }

    private static Specification<TechnicalPassport>? CreateSpecification(GetAllTechnicalPassportsQuery request)
    {
        Specification<TechnicalPassport>? specification = null;

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

        if (request.Request.FirstName is not null)
        {
            var firstNameMatchToSpecification = new FirstNameMatchToSpecification(request.Request.FirstName);

            if (specification is null)
            {
                specification = firstNameMatchToSpecification;
            }
            else
            {
                specification.And(firstNameMatchToSpecification);
            }
        }

        if (request.Request.LastName is not null)
        {
            var lastNameMatchToSpecification = new LastNameMatchToSpecification(request.Request.LastName);

            if (specification is null)
            {
                specification = lastNameMatchToSpecification;
            }
            else
            {
                specification.And(lastNameMatchToSpecification);
            }
        }

        if (request.Request.IssueDateFrom is not null)
        {
            var issueDateAtLeastSpecification =
                new IssueDateAtLeastSpecification(DateOnly.Parse(request.Request.IssueDateFrom));

            if (specification is null)
            {
                specification = issueDateAtLeastSpecification;
            }
            else
            {
                specification.And(issueDateAtLeastSpecification);
            }
        }

        if (request.Request.IssueDateTo is not null)
        {
            var issueDateAtMostSpecification =
                new IssueDateAtMostSpecification(DateOnly.Parse(request.Request.IssueDateTo));

            if (specification is null)
            {
                specification = issueDateAtMostSpecification;
            }
            else
            {
                specification.And(issueDateAtMostSpecification);
            }
        }

        if (request.Request.SAICode is not null)
        {
            var saiCodeMatchToSpecification = new SAICodeMatchToSpecification(request.Request.SAICode);

            if (specification is null)
            {
                specification = saiCodeMatchToSpecification;
            }
            else
            {
                specification.And(saiCodeMatchToSpecification);
            }
        }

        if (request.Request.LicensePlate is not null)
        {
            var licensePlateMatchToSpecification = new LicensePlateMatchToSpecification(request.Request.LicensePlate);

            if (specification is null)
            {
                specification = licensePlateMatchToSpecification;
            }
            else
            {
                specification.And(licensePlateMatchToSpecification);
            }
        }

        if (request.Request.Brand is not null)
        {
            var brandMatchToSpecification = new BrandMatchToSpecification(request.Request.Brand);

            if (specification is null)
            {
                specification = brandMatchToSpecification;
            }
            else
            {
                specification.And(brandMatchToSpecification);
            }
        }

        if (request.Request.Model is not null)
        {
            var modelMatchToSpecification = new ModelMatchToSpecification(request.Request.Model);

            if (specification is null)
            {
                specification = modelMatchToSpecification;
            }
            else
            {
                specification.And(modelMatchToSpecification);
            }
        }

        if (request.Request.CreationYearFrom.HasValue)
        {
            var creationYearAtLeastSpecification =
                new CreationYearAtLeastSpecification(request.Request.CreationYearFrom.Value);

            if (specification is null)
            {
                specification = creationYearAtLeastSpecification;
            }
            else
            {
                specification.And(creationYearAtLeastSpecification);
            }
        }

        if (request.Request.CreationYearTo.HasValue)
        {
            var creationYearAtMostSpecification =
                new CreationYearAtMostSpecification(request.Request.CreationYearTo.Value);

            if (specification is null)
            {
                specification = creationYearAtMostSpecification;
            }
            else
            {
                specification.And(creationYearAtMostSpecification);
            }
        }

        if (request.Request.VIN is not null)
        {
            var vinMatchToSpecification = new VINMatchToSpecification(request.Request.VIN);

            if (specification is null)
            {
                specification = vinMatchToSpecification;
            }
            else
            {
                specification.And(vinMatchToSpecification);
            }
        }

        if (request.Request.MaxWeightFrom.HasValue)
        {
            var maxWeightAtLeastSpecification = new MaxWeightAtLeastSpecification(request.Request.MaxWeightFrom.Value);

            if (specification is null)
            {
                specification = maxWeightAtLeastSpecification;
            }
            else
            {
                specification.And(maxWeightAtLeastSpecification);
            }
        }

        if (request.Request.MaxWeightTo.HasValue)
        {
            var maxWeightAtMostSpecification = new MaxWeightAtMostSpecification(request.Request.MaxWeightTo.Value);

            if (specification is null)
            {
                specification = maxWeightAtMostSpecification;
            }
            else
            {
                specification.And(maxWeightAtMostSpecification);
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
