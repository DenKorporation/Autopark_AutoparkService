namespace AutoparkService.Application.DTOs.Permission.Request;

public record PermissionRequest(
    string Number,
    string ExpiryDate,
    Guid VehicleId);
