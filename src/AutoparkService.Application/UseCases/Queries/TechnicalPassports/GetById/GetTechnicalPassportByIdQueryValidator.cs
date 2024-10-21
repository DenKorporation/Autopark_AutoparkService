using FluentValidation;

namespace AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetById;

public class GetTechnicalPassportByIdQueryValidator : AbstractValidator<GetTechnicalPassportByIdQuery>
{
    public GetTechnicalPassportByIdQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("TechnicalPassport Id was expected");
    }
}
