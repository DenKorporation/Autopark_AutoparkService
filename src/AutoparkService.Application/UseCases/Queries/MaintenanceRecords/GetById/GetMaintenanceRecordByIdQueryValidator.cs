using FluentValidation;

namespace AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetById;

public class GetMaintenanceRecordByIdQueryValidator : AbstractValidator<GetMaintenanceRecordByIdQuery>
{
    public GetMaintenanceRecordByIdQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("MaintenanceRecord Id was expected");
    }
}
