using AutoparkService.Application.Validators;
using FluentValidation;

namespace AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetAll;

public class GetAllTechnicalPassportsQueryValidator : AbstractValidator<GetAllTechnicalPassportsQuery>
{
    public GetAllTechnicalPassportsQueryValidator()
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

        RuleFor(x => x.Request.Number)
            .MaximumLength(9)
            .WithMessage("Length of technical passport number mustn't exceed 9")
            .When(x => x.Request.Number is not null);

        RuleFor(x => x.Request.FirstName)
            .MaximumLength(50)
            .WithMessage("Length of first name mustn't exceed 50")
            .When(x => x.Request.FirstName is not null);

        RuleFor(x => x.Request.LastName)
            .MaximumLength(50)
            .WithMessage("Length of last name mustn't exceed 50")
            .When(x => x.Request.LastName is not null);

        RuleFor(x => x.Request.IssueDateFrom)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.Request.IssueDateFrom is not null);

        RuleFor(x => x.Request.IssueDateTo)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.Request.IssueDateTo is not null);

        RuleFor(x => x.Request.SAICode)
            .MaximumLength(6)
            .WithMessage("Length of technical passport SAI code mustn't exceed 6")
            .When(x => x.Request.SAICode is not null);

        RuleFor(x => x.Request.LicensePlate)
            .MaximumLength(8)
            .WithMessage("Length of license plate mustn't exceed 8")
            .When(x => x.Request.LicensePlate is not null);

        RuleFor(x => x.Request.Brand)
            .MaximumLength(30)
            .WithMessage("Length of vehicle brand mustn't exceed 30")
            .When(x => x.Request.Brand is not null);

        RuleFor(x => x.Request.Model)
            .MaximumLength(50)
            .WithMessage("Length of vehicle model mustn't exceed 50")
            .When(x => x.Request.Model is not null);

        RuleFor(x => x.Request.CreationYearFrom)
            .GreaterThan(1900U)
            .WithMessage("Creation year of vehicle greater than 1900")
            .When(x => x.Request.CreationYearFrom.HasValue);

        RuleFor(x => x.Request.CreationYearTo)
            .GreaterThan(1900U)
            .WithMessage("Creation year of vehicle greater than 1900")
            .When(x => x.Request.CreationYearTo.HasValue);

        RuleFor(x => x.Request.VIN)
            .MaximumLength(17)
            .WithMessage("Length of technical passport VIN code mustn't exceed 17")
            .When(x => x.Request.VIN is not null);

        RuleFor(x => x.Request.MaxWeightFrom)
            .GreaterThanOrEqualTo(0U)
            .WithMessage("Vehicle max weight shouldn't be negative")
            .When(x => x.Request.MaxWeightFrom.HasValue);

        RuleFor(x => x.Request.MaxWeightTo)
            .GreaterThanOrEqualTo(0U)
            .WithMessage("Vehicle max weight shouldn't be negative")
            .When(x => x.Request.MaxWeightTo.HasValue);
    }
}
