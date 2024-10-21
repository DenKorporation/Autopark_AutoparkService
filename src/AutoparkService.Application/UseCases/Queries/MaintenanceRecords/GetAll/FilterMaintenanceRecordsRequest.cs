namespace AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetAll;

public record FilterMaintenanceRecordsRequest(
    int PageNumber,
    int PageSize,
    string? Type,
    string? StartDate,
    string? EndDate,
    int? OdometerFrom,
    int? OdometerTo,
    string? ServiceCenter,
    decimal? CostFrom,
    decimal? CostTo,
    Guid? VehicleId);
