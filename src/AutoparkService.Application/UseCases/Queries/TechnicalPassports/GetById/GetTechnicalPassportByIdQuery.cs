using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetById;

public record GetTechnicalPassportByIdQuery(Guid Id)
    : IQuery<TechnicalPassportResponse>;
