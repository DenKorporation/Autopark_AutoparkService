using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Insurance;
using AutoparkService.Application.Messaging;
using AutoparkService.Domain.Repositories.Interfaces;
using FluentResults;

namespace AutoparkService.Application.UseCases.Commands.Insurances.Delete;

public class DeleteInsuranceHandler(IInsuranceRepository insuranceRepository)
    : ICommandHandler<DeleteInsuranceCommand>
{
    public async Task<Result> Handle(DeleteInsuranceCommand request, CancellationToken cancellationToken)
    {
        var insurance = await insuranceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (insurance is null)
        {
            return new InsuranceNotFoundError(request.Id);
        }

        try
        {
            await insuranceRepository.DeleteAsync(insurance, cancellationToken);
        }
        catch (Exception)
        {
            return new InternalServerError("Insurance.Delete");
        }

        return Result.Ok();
    }
}
