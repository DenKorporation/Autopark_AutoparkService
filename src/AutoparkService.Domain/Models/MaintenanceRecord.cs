namespace AutoparkService.Domain.Models;

public class MaintenanceRecord
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public uint Odometer { get; set; }
    public string ServiceCenter { get; set; } = null!;
    public decimal Cost { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
}
