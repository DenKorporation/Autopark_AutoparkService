using FluentValidation;

namespace AutoparkService.Application.UseCases.Commands.Permissions.Delete;

public class DeletePermissionCommandValidator : AbstractValidator<DeletePermissionCommand>
{
    public DeletePermissionCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Permission Id was expected");
    }
}
