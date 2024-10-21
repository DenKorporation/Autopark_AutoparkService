using AutoparkService.Application.Validators;
using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.Insurances.Update;

public class UpdateInsuranceCommandValidator : AbstractValidator<UpdateInsuranceCommand>
{
    public UpdateInsuranceCommandValidator()
    {
        RuleFor(c => c.Request)
            .SetValidator(new InsuranceRequestValidator());

        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Insurance Id was expected");
    }
}
