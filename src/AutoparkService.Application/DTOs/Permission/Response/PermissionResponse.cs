namespace AutoparkService.Application.DTOs.Permission.Response;

public record PermissionResponse(
    Guid Id,
    string Number,
    DateOnly ExpiryDate,
    Guid VehicleId,
    bool IsValid);
