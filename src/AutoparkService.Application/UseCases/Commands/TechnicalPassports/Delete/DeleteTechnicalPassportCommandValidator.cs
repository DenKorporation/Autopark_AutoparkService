using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.TechnicalPassports.Delete;

public class DeleteTechnicalPassportCommandValidator : AbstractValidator<DeleteTechnicalPassportCommand>
{
    public DeleteTechnicalPassportCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("TechnicalPassport Id was expected");
    }
}
