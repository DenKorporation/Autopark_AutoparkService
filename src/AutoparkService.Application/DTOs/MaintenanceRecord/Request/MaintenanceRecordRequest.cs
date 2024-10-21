namespace AutoparkService.Application.DTOs.MaintenanceRecord.Request;

public record MaintenanceRecordRequest(
    string Type,
    string StartDate,
    string? EndDate,
    uint Odometer,
    string ServiceCenter,
    decimal Cost,
    Guid VehicleId);
