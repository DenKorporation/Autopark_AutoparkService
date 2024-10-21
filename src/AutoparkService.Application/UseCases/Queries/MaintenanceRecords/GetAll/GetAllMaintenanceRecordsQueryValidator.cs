using AutoparkService.Application.Validators;
using FluentValidation;

namespace AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetAll;

public class GetAllMaintenanceRecordsQueryValidator : AbstractValidator<GetAllMaintenanceRecordsQuery>
{
    public GetAllMaintenanceRecordsQueryValidator()
    {
        RuleFor(x => x.Request.PageNumber)
            .NotNull()
            .WithMessage("The page number was expected")
            .GreaterThan(0)
            .WithMessage("The page number must be more than 0");

        RuleFor(x => x.Request.PageSize)
            .NotNull()
            .WithMessage("The page size was expected")
            .GreaterThan(0)
            .WithMessage("The page size must be more than 0");

        RuleFor(mr => mr.Request.Type)
            .MaximumLength(100)
            .WithMessage("Length of maintenance type mustn't exceed 100")
            .When(x => x.Request.Type is not null);

        RuleFor(x => x.Request.StartDate)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.Request.StartDate is not null);

        RuleFor(x => x.Request.EndDate)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.Request.EndDate is not null);

        RuleFor(x => x.Request.OdometerFrom)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle odometer status shouldn't be negative")
            .When(x => x.Request.OdometerFrom.HasValue);

        RuleFor(x => x.Request.OdometerTo)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle odometer status shouldn't be negative")
            .When(x => x.Request.OdometerTo.HasValue);

        RuleFor(mr => mr.Request.ServiceCenter)
            .MaximumLength(255)
            .WithMessage("Length of service center mustn't exceed 255")
            .When(x => x.Request.ServiceCenter is not null);

        RuleFor(x => x.Request.CostFrom)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maintenance cost shouldn't be negative")
            .When(x => x.Request.CostFrom.HasValue);

        RuleFor(x => x.Request.CostTo)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maintenance cost shouldn't be negative")
            .When(x => x.Request.CostTo.HasValue);
    }
}
