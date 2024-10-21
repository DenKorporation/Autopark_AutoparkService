using FluentValidation;

namespace AutoparkService.Application.UseCases.Queries.Permissions.GetAll;

public class GetAllPermissionsQueryValidator : AbstractValidator<GetAllPermissionsQuery>
{
    public GetAllPermissionsQueryValidator()
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

        RuleFor(p => p.Request.Number)
            .MaximumLength(9)
            .WithMessage("Length of permission number mustn't exceed 9")
            .When(x => x.Request.Number is not null);

        RuleFor(x => x.Request.ExpiryDateFrom)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.Request.ExpiryDateFrom is not null);

        RuleFor(x => x.Request.ExpiryDateTo)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.Request.ExpiryDateTo is not null);
    }
}
