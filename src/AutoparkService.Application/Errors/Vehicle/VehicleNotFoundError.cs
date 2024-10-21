using AutoparkService.Application.Errors.Base;

namespace AutoparkService.Application.Errors.Vehicle;

public class VehicleNotFoundError(string code = "Vehicle.NotFound", string message = "Vehicle not found")
    : NotFoundError(code, message)
{
    public VehicleNotFoundError(Guid vehicleId)
        : this(message: $"Vehicle '{vehicleId}' not found")
    {
    }
}
