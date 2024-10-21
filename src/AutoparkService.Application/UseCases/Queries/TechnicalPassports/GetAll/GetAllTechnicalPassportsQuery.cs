using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetAll;

public record GetAllTechnicalPassportsQuery(FilterTechnicalPassportsRequest Request)
    : IQuery<PagedList<TechnicalPassportResponse>>;
