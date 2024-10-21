using AutoparkService.Application.DTOs.Common;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Queries.Insurances.GetAll;

public record GetAllInsurancesQuery(FilterInsurancesRequest Request) : IQuery<PagedList<InsuranceResponse>>;
