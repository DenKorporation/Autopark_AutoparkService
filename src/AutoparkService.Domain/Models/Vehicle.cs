namespace AutoparkService.Domain.Models;

public class Vehicle
{
    public Guid Id { get; set; }
    public uint Odometer { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public decimal Cost { get; set; }
    public string Status { get; set; } = null!;
    public Guid UserId { get; set; }
    public TechnicalPassport? TechnicalPassport { get; set; }
    public Permission? Permission { get; set; }
    public ICollection<Insurance> Insurances { get; set; } = null!;
    public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = null!;
}
