using FluentValidation;

namespace AutoparkService.Application.UseCases.Queries.Vehicles.GetAll;

public class GetAllVehiclesQueryValidator : AbstractValidator<GetAllVehiclesQuery>
{
    public GetAllVehiclesQueryValidator()
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

        RuleFor(x => x.Request.OdometerFrom)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle odometer status shouldn't be negative")
            .When(x => x.Request.OdometerFrom.HasValue);

        RuleFor(x => x.Request.OdometerTo)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle odometer status shouldn't be negative")
            .When(x => x.Request.OdometerTo.HasValue);

        RuleFor(x => x.Request.PurchaseDateFrom)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.Request.PurchaseDateFrom is not null);

        RuleFor(x => x.Request.PurchaseDateTo)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.Request.PurchaseDateTo is not null);

        RuleFor(x => x.Request.CostFrom)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle cost shouldn't be negative")
            .When(x => x.Request.CostFrom.HasValue);

        RuleFor(x => x.Request.CostTo)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle cost shouldn't be negative")
            .When(x => x.Request.CostTo.HasValue);

        RuleFor(v => v.Request.Status)
            .MaximumLength(50)
            .WithMessage("Length of vehicle status mustn't exceed 50")
            .When(x => x.Request.Status is not null);
    }
}
