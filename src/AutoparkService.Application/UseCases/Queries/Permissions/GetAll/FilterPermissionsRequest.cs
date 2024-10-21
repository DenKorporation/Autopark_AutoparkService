namespace AutoparkService.Application.UseCases.Queries.Permissions.GetAll;

public record FilterPermissionsRequest(
    int PageNumber,
    int PageSize,
    string? Number,
    string? ExpiryDateFrom,
    string? ExpiryDateTo,
    Guid? VehicleId,
    bool? IsValid);
