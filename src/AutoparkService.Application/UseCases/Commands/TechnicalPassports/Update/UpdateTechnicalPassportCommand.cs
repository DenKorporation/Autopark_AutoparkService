using AutoparkService.Application.DTOs.TechnicalPassport.Request;
using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.TechnicalPassports.Update;

public record UpdateTechnicalPassportCommand(Guid Id, TechnicalPassportRequest Request)
    : ICommand<TechnicalPassportResponse>;
