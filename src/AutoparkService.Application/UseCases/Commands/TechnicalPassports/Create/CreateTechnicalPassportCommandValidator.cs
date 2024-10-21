using AutoparkService.Application.Validators;
using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.TechnicalPassports.Create;

public class CreateTechnicalPassportCommandValidator : AbstractValidator<CreateTechnicalPassportCommand>
{
    public CreateTechnicalPassportCommandValidator()
    {
        RuleFor(c => c.Request)
            .SetValidator(new TechnicalPassportRequestValidator());
    }
}
