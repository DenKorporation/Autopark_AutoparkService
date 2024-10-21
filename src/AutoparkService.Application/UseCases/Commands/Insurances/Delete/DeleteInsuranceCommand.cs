using AutoparkService.Application.Messaging;

namespace AutoparkService.Application.UseCases.Commands.Insurances.Delete;

public record DeleteInsuranceCommand(Guid Id)
    : ICommand;
