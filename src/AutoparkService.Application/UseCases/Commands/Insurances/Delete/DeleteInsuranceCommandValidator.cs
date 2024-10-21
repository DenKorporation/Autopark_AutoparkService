using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.Insurances.Delete;

public class DeleteInsuranceCommandValidator : AbstractValidator<DeleteInsuranceCommand>
{
    public DeleteInsuranceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Insurance Id was expected");
    }
}
