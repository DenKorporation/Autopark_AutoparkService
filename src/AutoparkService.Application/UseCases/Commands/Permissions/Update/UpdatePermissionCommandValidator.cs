using AutoparkService.Application.Validators;
using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.Permissions.Update;

public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(c => c.Request)
            .SetValidator(new PermissionRequestValidator());

        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Permission Id was expected");
    }
}
