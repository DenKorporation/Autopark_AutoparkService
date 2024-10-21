namespace AutoparkService.Application.UseCases.Queries.Insurances.GetAll;

public record FilterInsurancesRequest(
    int PageNumber,
    int PageSize,
    string? Series,
    string? Number,
    string? VehicleType,
    string? Provider,
    string? StartDate,
    string? EndDate,
    decimal? CostFrom,
    decimal? CostTo,
    Guid? VehicleId,
    bool? IsValid);
