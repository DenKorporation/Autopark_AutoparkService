namespace AutoparkService.Application.UseCases.Queries.Vehicles.GetAll;

public record FilterVehiclesRequest(
    int PageNumber,
    int PageSize,
    int? OdometerFrom,
    int? OdometerTo,
    string? PurchaseDateFrom,
    string? PurchaseDateTo,
    decimal? CostFrom,
    decimal? CostTo,
    string? Status,
    Guid? UserId);
