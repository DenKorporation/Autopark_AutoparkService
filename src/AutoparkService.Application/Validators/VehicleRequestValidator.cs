using AutoparkService.Application.DTOs.Vehicle.Request;
using FluentValidation;

namespace AutoparkService.Application.Validators;

public class VehicleRequestValidator : AbstractValidator<VehicleRequest>
{
    public VehicleRequestValidator()
    {
        RuleFor(x => x.Odometer)
            .NotEmpty()
            .WithMessage("Odometer state was expected")
            .GreaterThanOrEqualTo(0U)
            .WithMessage("Odometer state shouldn't be negative");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty()
            .WithMessage("Vehicle purchase date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

        RuleFor(x => x.Cost)
            .NotEmpty()
            .WithMessage("Vehicle cost was expected")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle cost shouldn't be negative");

        RuleFor(i => i.Status)
            .NotEmpty()
            .WithMessage("Vehicle status was expected")
            .MaximumLength(50)
            .WithMessage("Length of vehicle status mustn't exceed 50");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID was expected");
    }
}
