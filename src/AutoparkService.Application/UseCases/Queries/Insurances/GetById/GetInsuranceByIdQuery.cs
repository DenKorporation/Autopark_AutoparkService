using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Queries.Insurances.GetById;

public record GetInsuranceByIdQuery(Guid Id) : IQuery<InsuranceResponse>;
