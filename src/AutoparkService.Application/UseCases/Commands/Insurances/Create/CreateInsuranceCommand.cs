using AutoparkService.Application.DTOs.Insurance.Request;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.Insurances.Create;

public record CreateInsuranceCommand(InsuranceRequest Request)
    : ICommand<InsuranceResponse>;
