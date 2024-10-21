using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Delete;

public class DeleteMaintenanceRecordCommandValidator : AbstractValidator<DeleteMaintenanceRecordCommand>
{
    public DeleteMaintenanceRecordCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("MaintenanceRecord Id was expected");
    }
}
