using AutoparkService.Application.DTOs.TechnicalPassport.Request;
using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.TechnicalPassports.Create;

public record CreateTechnicalPassportCommand(TechnicalPassportRequest Request) : ICommand<TechnicalPassportResponse>;
