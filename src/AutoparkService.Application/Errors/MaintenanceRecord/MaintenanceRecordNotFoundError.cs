using AutoparkService.Application.Errors.Base;

namespace AutoparkService.Application.Errors.MaintenanceRecord;

public class MaintenanceRecordNotFoundError(
    string code = "MaintenanceRecord.NotFound",
    string message = "MaintenanceRecord not found")
    : NotFoundError(code, message)
{
    public MaintenanceRecordNotFoundError(Guid maintenanceRecordId)
        : this(message: $"MaintenanceRecord '{maintenanceRecordId}' not found")
    {
    }
}
