using AutoMapper;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Application.Errors.Insurance;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Queries.Insurances.GetById;

public class GetInsuranceByIdHandler(
    IMapper mapper,
    IInsuranceRepository insuranceRepository)
    : IQueryHandler<GetInsuranceByIdQuery, InsuranceResponse>
{
    public async Task<Result<InsuranceResponse>> Handle(
        GetInsuranceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var insurance = await insuranceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (insurance is null)
        {
            return new InsuranceNotFoundError(request.Id);
        }

        return mapper.Map<InsuranceResponse>(insurance);
    }
}
