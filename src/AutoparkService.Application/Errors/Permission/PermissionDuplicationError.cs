using AutoparkService.Application.Errors.Base;

namespace AutoparkService.Application.Errors.Permission;

public class PermissionDuplicationError(
    string code = "Permission.Duplication",
    string message = "Permission already exist")
    : ConflictError(code, message)
{
    public PermissionDuplicationError(Guid vehicleId)
        : this(message: $"Permission for vehicle '{vehicleId}' already exist")
    {
    }

    public PermissionDuplicationError(string number)
        : this(message: $"Permission '{number}' already exist")
    {
    }
}
