using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.TechnicalPassport;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.TechnicalPassports.Delete;

public class DeleteTechnicalPassportHandler(
    ITechnicalPassportRepository technicalPassportRepository)
    : ICommandHandler<DeleteTechnicalPassportCommand>
{
    public async Task<Result> Handle(DeleteTechnicalPassportCommand request, CancellationToken cancellationToken)
    {
        var technicalPassport = await technicalPassportRepository.GetByIdAsync(request.Id, cancellationToken);

        if (technicalPassport is null)
        {
            return new TechnicalPassportNotFoundError(request.Id);
        }

        try
        {
            await technicalPassportRepository.DeleteAsync(technicalPassport, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("TechnicalPassport.Delete");
        }

        return Result.Ok();
    }
}
