namespace AutoparkService.Application.DTOs.Vehicle.Response;

public record VehicleResponse(
    Guid Id,
    uint Odometer,
    DateOnly PurchaseDate,
    decimal Cost,
    string Status,
    Guid UserId,
    Guid? TechnicalPassportId,
    Guid? PermissionId,
    IEnumerable<Guid> InsuranceIds,
    IEnumerable<Guid> MaintenanceRecordIds);