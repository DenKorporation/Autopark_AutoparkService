using AutoparkService.Application.DTOs.Insurance.Request;
using FluentValidation;

namespace AutoparkService.Application.Validators;

public class InsuranceRequestValidator : AbstractValidator<InsuranceRequest>
{
    public InsuranceRequestValidator()
    {
        RuleFor(i => i.Series)
            .NotEmpty()
            .WithMessage("Insurance series was expected")
            .Length(2)
            .WithMessage("Length of insurance series was expected to be 2")
            .Matches("^[a-zA-Z]+$")
            .WithMessage("Insurance series must contain only alphabetic letters");

        RuleFor(i => i.Number)
            .NotEmpty()
            .WithMessage("Insurance number was expected")
            .Length(7)
            .WithMessage("Length of insurance number was expected to be 7")
            .Matches("^[0-9]+$")
            .WithMessage("Insurance number must contain only digits");

        RuleFor(i => i.VehicleType)
            .NotEmpty()
            .WithMessage("Vehicle type was expected")
            .Length(2)
            .WithMessage("Length of vehicle type was expected to be 2")
            .Matches("^[a-zA-Z][0-9]$")
            .WithMessage("Vehicle type must contain alphabetic letter and digit");

        RuleFor(i => i.Provider)
            .NotEmpty()
            .WithMessage("Insurance provider was expected")
            .MaximumLength(255)
            .WithMessage("Length of insurance provider mustn't exceed 255");

        RuleFor(x => x.IssueDate)
            .NotEmpty()
            .WithMessage("Insurance issue date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Insurance start date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("Insurance end date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

        RuleFor(x => x.Cost)
            .NotEmpty()
            .WithMessage("Insurance cost was expected")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Insurance cost shouldn't be negative");

        RuleFor(x => x.VehicleId)
            .NotEmpty()
            .WithMessage("Vehicle ID was expected");
    }
}
