using AutoMapper;
using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Application.Errors.TechnicalPassport;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetById;

public class GetTechnicalPassportByIdHandler(
    IMapper mapper,
    ITechnicalPassportRepository technicalPassportRepository)
    : IQueryHandler<GetTechnicalPassportByIdQuery, TechnicalPassportResponse>
{
    public async Task<Result<TechnicalPassportResponse>> Handle(
        GetTechnicalPassportByIdQuery request,
        CancellationToken cancellationToken)
    {
        var technicalPassport = await technicalPassportRepository.GetByIdAsync(request.Id, cancellationToken);

        if (technicalPassport is null)
        {
            return new TechnicalPassportNotFoundError(request.Id);
        }

        return mapper.Map<TechnicalPassportResponse>(technicalPassport);
    }
}
