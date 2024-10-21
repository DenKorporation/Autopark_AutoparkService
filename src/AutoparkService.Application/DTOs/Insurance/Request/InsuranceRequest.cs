namespace AutoparkService.Application.DTOs.Insurance.Request;

public record InsuranceRequest(
    string Series,
    string Number,
    string VehicleType,
    string Provider,
    string IssueDate,
    string StartDate,
    string EndDate,
    decimal Cost,
    Guid VehicleId);
