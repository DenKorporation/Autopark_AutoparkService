using AutoparkService.Application.DTOs.Insurance.Request;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.Insurances.Update;

public record UpdateInsuranceCommand(Guid Id, InsuranceRequest Request) : ICommand<InsuranceResponse>;
