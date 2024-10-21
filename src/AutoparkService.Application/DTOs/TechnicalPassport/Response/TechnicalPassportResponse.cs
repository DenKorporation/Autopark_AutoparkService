namespace AutoparkService.Application.DTOs.TechnicalPassport.Response;

public record TechnicalPassportResponse(
    Guid Id,
    string Number,
    string FirstName,
    string FirstNameLatin,
    string LastName,
    string LastNameLatin,
    string Patronymic,
    string Address,
    DateOnly IssueDate,
    string SAICode,
    string LicensePlate,
    string Brand,
    string Model,
    uint CreationYear,
    string Color,
    string VIN,
    string VehicleType,
    uint MaxWeight,
    Guid VehicleId);