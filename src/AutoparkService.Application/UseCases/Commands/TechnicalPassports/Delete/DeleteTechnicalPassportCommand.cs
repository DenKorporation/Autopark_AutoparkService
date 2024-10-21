using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.TechnicalPassports.Delete;

public record DeleteTechnicalPassportCommand(Guid Id) : ICommand;
