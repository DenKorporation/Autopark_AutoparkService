using FluentValidation;

namespace AutoparkService.Application.UseCases.Queries.Vehicles.GetById;

public class GetVehicleByIdQueryValidator : AbstractValidator<GetVehicleByIdQuery>
{
    public GetVehicleByIdQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Vehicle Id was expected");
    }
}
