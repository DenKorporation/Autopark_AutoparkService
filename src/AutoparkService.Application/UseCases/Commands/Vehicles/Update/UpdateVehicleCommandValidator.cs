using AutoparkService.Application.Validators;
using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.Vehicles.Update;

public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
    public UpdateVehicleCommandValidator()
    {
        RuleFor(c => c.Request)
            .SetValidator(new VehicleRequestValidator());

        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Vehicle Id was expected");
    }
}
