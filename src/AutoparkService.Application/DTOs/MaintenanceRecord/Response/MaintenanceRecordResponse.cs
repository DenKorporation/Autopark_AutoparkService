namespace AutoparkService.Application.DTOs.MaintenanceRecord.Response;

public record MaintenanceRecordResponse(
    Guid Id,
    string Type,
    DateOnly StartDate,
    DateOnly? EndDate,
    uint Odometer,
    string ServiceCenter,
    decimal Cost,
    Guid VehicleId);