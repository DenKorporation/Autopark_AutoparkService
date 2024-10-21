namespace AutoparkService.Domain.Models;

public class Permission
{
    public Guid Id { get; set; }
    public string Number { get; set; } = null!;
    public DateOnly ExpiryDate { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
}
