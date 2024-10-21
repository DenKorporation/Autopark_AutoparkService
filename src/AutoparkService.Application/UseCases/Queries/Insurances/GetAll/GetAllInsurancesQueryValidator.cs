using FluentValidation;

namespace AutoparkService.Application.UseCases.Queries.Insurances.GetAll;

public class GetAllInsurancesQueryValidator : AbstractValidator<GetAllInsurancesQuery>
{
    public GetAllInsurancesQueryValidator()
    {
        RuleFor(x => x.Request.PageNumber)
            .NotNull()
            .WithMessage("The page number was expected")
            .GreaterThan(0)
            .WithMessage("The page number must be more than 0");

        RuleFor(x => x.Request.PageSize)
            .NotNull()
            .WithMessage("The page size was expected")
            .GreaterThan(0)
            .WithMessage("The page size must be more than 0");

        RuleFor(i => i.Request.Series)
            .MaximumLength(2)
            .WithMessage("Length of insurance series mustn't exceed 2")
            .When(x => x.Request.Series is not null);

        RuleFor(i => i.Request.Number)
            .MaximumLength(7)
            .WithMessage("Length of insurance number mustn't exceed 7")
            .When(x => x.Request.Number is not null);

        RuleFor(i => i.Request.VehicleType)
            .MaximumLength(2)
            .WithMessage("Length of vehicle type mustn't exceed 2")
            .When(x => x.Request.VehicleType is not null);

        RuleFor(i => i.Request.Provider)
            .MaximumLength(255)
            .WithMessage("Length of insurance provider mustn't exceed 255")
            .When(x => x.Request.Provider is not null);

        RuleFor(x => x.Request.StartDate)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.Request.StartDate is not null);

        RuleFor(x => x.Request.EndDate)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.Request.EndDate is not null);

        RuleFor(x => x.Request.CostFrom)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle cost shouldn't be negative")
            .When(x => x.Request.CostFrom.HasValue);

        RuleFor(x => x.Request.CostTo)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Insurance cost shouldn't be negative")
            .When(x => x.Request.CostTo.HasValue);
    }
}
