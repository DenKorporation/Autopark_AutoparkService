namespace AutoparkService.Application.DTOs.Insurance.Response;

public record InsuranceResponse(
    Guid Id,
    string Series,
    string Number,
    string VehicleType,
    string Provider,
    DateOnly IssueDate,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal Cost,
    Guid VehicleId,
    bool IsValid);