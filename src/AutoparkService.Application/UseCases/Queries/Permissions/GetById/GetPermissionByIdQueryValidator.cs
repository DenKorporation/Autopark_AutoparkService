using FluentValidation;

namespace AutoparkService.Application.UseCases.Queries.Permissions.GetById;

public class GetPermissionByIdQueryValidator : AbstractValidator<GetPermissionByIdQuery>
{
    public GetPermissionByIdQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Permission Id was expected");
    }
}
