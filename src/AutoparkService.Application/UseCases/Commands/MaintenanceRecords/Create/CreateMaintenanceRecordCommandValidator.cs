using AutoparkService.Application.Validators;
using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Create;

public class CreateMaintenanceRecordCommandValidator : AbstractValidator<CreateMaintenanceRecordCommand>
{
    public CreateMaintenanceRecordCommandValidator()
    {
        RuleFor(c => c.Request)
            .SetValidator(new MaintenanceRecordsRequestValidator());
    }
}
