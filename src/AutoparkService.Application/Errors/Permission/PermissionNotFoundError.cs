using AutoparkService.Application.Errors.Base;

namespace AutoparkService.Application.Errors.Permission;

public class PermissionNotFoundError(string code = "Permission.NotFound", string message = "Permission not found")
    : NotFoundError(code, message)
{
    public PermissionNotFoundError(Guid permissionId)
        : this(message: $"Permission '{permissionId}' not found")
    {
    }
}
