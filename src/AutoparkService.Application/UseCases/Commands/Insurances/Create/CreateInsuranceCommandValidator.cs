using AutoparkService.Application.Validators;
using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.Insurances.Create;

public class CreateInsuranceCommandValidator : AbstractValidator<CreateInsuranceCommand>
{
    public CreateInsuranceCommandValidator()
    {
        RuleFor(c => c.Request)
            .SetValidator(new InsuranceRequestValidator());
    }
}
