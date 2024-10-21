namespace AutoparkService.Application.DTOs.TechnicalPassport.Request;

public record TechnicalPassportRequest(
    string Number,
    string FirstName,
    string FirstNameLatin,
    string LastName,
    string LastNameLatin,
    string Patronymic,
    string Address,
    string IssueDate,
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
