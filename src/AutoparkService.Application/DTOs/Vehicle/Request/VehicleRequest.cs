namespace AutoparkService.Application.DTOs.Vehicle.Request;

public record VehicleRequest(
    uint Odometer,
    string PurchaseDate,
    decimal Cost,
    string Status,
    Guid UserId);
