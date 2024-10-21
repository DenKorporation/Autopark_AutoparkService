using AutoparkService.Application.DTOs.Permission.Request;
using FluentValidation;

namespace AutoparkService.Application.Validators;

public class PermissionRequestValidator : AbstractValidator<PermissionRequest>
{
    public PermissionRequestValidator()
    {
        RuleFor(i => i.Number)
            .NotEmpty()
            .WithMessage("Permission number was expected")
            .Length(9)
            .WithMessage("Length of permission number was expected to be 9")
            .Matches("^[a-zA-Z]{2}[0-9]{7}$")
            .WithMessage("Permission number must contain 2 alphabetic letters and 7 digits");

        RuleFor(x => x.ExpiryDate)
            .NotEmpty()
            .WithMessage("Permission expiry date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

        RuleFor(x => x.VehicleId)
            .NotEmpty()
            .WithMessage("Vehicle ID was expected");
    }
}
