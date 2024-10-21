using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.Vehicles.Delete;

public class DeleteVehicleCommandValidator : AbstractValidator<DeleteVehicleCommand>
{
    public DeleteVehicleCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Vehicle Id was expected");
    }
}
