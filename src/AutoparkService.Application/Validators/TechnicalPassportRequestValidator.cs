using AutoparkService.Application.DTOs.TechnicalPassport.Request;
using FluentValidation;

namespace AutoparkService.Application.Validators;

public class TechnicalPassportRequestValidator : AbstractValidator<TechnicalPassportRequest>
{
    public TechnicalPassportRequestValidator()
    {
        RuleFor(i => i.Number)
            .NotEmpty()
            .WithMessage("Technical passport number was expected")
            .Length(9)
            .WithMessage("Length of technical passport number was expected to be 9")
            .Matches("^[a-zA-Z]{3}[0-9]{6}$")
            .WithMessage("Technical passport series must contain 3 alphabetic letters and 6 digits");

        RuleFor(i => i.FirstName)
            .NotEmpty()
            .WithMessage("Technical passport first name was expected")
            .MaximumLength(50)
            .WithMessage("Length of technical passport first name mustn't exceed 50");

        RuleFor(i => i.FirstNameLatin)
            .NotEmpty()
            .WithMessage("Technical passport first name latin was expected")
            .MaximumLength(50)
            .WithMessage("Length of technical passport first name latin mustn't exceed 50");

        RuleFor(i => i.LastName)
            .NotEmpty()
            .WithMessage("Technical passport last name was expected")
            .MaximumLength(50)
            .WithMessage("Length of technical passport last name mustn't exceed 50");

        RuleFor(i => i.LastName)
            .NotEmpty()
            .WithMessage("Technical passport last name latin was expected")
            .MaximumLength(50)
            .WithMessage("Length of technical passport last name latin mustn't exceed 50");

        RuleFor(i => i.Patronymic)
            .NotEmpty()
            .WithMessage("Technical passport patronymic was expected")
            .MaximumLength(50)
            .WithMessage("Length of technical passport patronymic mustn't exceed 50");

        RuleFor(i => i.Address)
            .NotEmpty()
            .WithMessage("Technical passport address was expected")
            .MaximumLength(255)
            .WithMessage("Length of technical passport address mustn't exceed 255");

        RuleFor(x => x.IssueDate)
            .NotEmpty()
            .WithMessage("Technical passport issue date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

        RuleFor(i => i.SAICode)
            .NotEmpty()
            .WithMessage("SAI code was expected")
            .Length(6)
            .WithMessage("Length of SAI code was expected to be 6")
            .Matches("^[0-9]{3}-[0-9]{2}$")
            .WithMessage("Invalid SAI code format. Expected format is 'ddd-dd'");

        RuleFor(i => i.LicensePlate)
            .NotEmpty()
            .WithMessage("License plate was expected")
            .Length(8)
            .WithMessage("Length of license plate was expected to be 8")
            .Matches("^[0-9]{4}[a-zA-Z]{2}-[0-9]$")
            .WithMessage("Invalid license plate format. Expected format is '1111AA-1'");

        RuleFor(i => i.Brand)
            .NotEmpty()
            .WithMessage("Vehicle brand was expected")
            .MaximumLength(30)
            .WithMessage("Length of vehicle brand mustn't exceed 30");

        RuleFor(i => i.Model)
            .NotEmpty()
            .WithMessage("Vehicle model was expected")
            .MaximumLength(50)
            .WithMessage("Length of vehicle model mustn't exceed 50");

        RuleFor(x => x.CreationYear)
            .NotEmpty()
            .WithMessage("Creation year of vehicle was expected")
            .GreaterThan(1900U)
            .WithMessage("Creation year of vehicle greater than 1900");

        RuleFor(i => i.Color)
            .NotEmpty()
            .WithMessage("Vehicle color was expected")
            .MaximumLength(100)
            .WithMessage("Length of vehicle color mustn't exceed 100");

        RuleFor(i => i.VIN)
            .NotEmpty()
            .WithMessage("VIN code was expected")
            .Length(17)
            .WithMessage("Length of VIN code was expected to be 17")
            .Matches("[a-zA-Z0-9]$")
            .WithMessage("VIN code must contain only alphabetic letters and number");

        RuleFor(i => i.VehicleType)
            .NotEmpty()
            .WithMessage("Vehicle type was expected")
            .MaximumLength(50)
            .WithMessage("Length of vehicle type mustn't exceed 50");

        RuleFor(x => x.MaxWeight)
            .NotEmpty()
            .WithMessage("Max weight of vehicle was expected")
            .GreaterThanOrEqualTo(0U)
            .WithMessage("Max weight of vehicle shouldn't be negative");

        RuleFor(x => x.VehicleId)
            .NotEmpty()
            .WithMessage("Vehicle ID was expected");
    }
}
