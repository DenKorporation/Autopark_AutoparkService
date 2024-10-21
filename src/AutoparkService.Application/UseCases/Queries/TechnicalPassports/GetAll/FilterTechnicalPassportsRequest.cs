namespace AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetAll;

public record FilterTechnicalPassportsRequest(
    int PageNumber,
    int PageSize,
    string? Number,
    string? FirstName,
    string? LastName,
    string? IssueDateFrom,
    string? IssueDateTo,
    string? SAICode,
    string? LicensePlate,
    string? Brand,
    string? Model,
    uint? CreationYearFrom,
    uint? CreationYearTo,
    string? VIN,
    uint? MaxWeightFrom,
    uint? MaxWeightTo,
    Guid? VehicleId);
