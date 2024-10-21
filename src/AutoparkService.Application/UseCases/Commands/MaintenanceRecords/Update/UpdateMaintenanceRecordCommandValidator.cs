using AutoparkService.Application.Validators;
using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Update;

public class UpdateMaintenanceRecordCommandValidator : AbstractValidator<UpdateMaintenanceRecordCommand>
{
    public UpdateMaintenanceRecordCommandValidator()
    {
        RuleFor(c => c.Request)
            .SetValidator(new MaintenanceRecordsRequestValidator());

        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("MaintenanceRecord Id was expected");
    }
}
