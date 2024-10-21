using AutoparkService.Application.Validators;
using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.TechnicalPassports.Update;

public class UpdateTechnicalPassportCommandValidator : AbstractValidator<UpdateTechnicalPassportCommand>
{
    public UpdateTechnicalPassportCommandValidator()
    {
        RuleFor(c => c.Request)
            .SetValidator(new TechnicalPassportRequestValidator());

        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("TechnicalPassport Id was expected");
    }
}
