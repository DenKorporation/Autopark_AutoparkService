using FluentValidation;

namespace AutoparkService.Application.UseCases.Queries.Insurances.GetById;

public class GetInsuranceByIdQueryValidator : AbstractValidator<GetInsuranceByIdQuery>
{
    public GetInsuranceByIdQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Insurance Id was expected");
    }
}
