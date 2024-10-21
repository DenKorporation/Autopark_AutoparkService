namespace AutoparkService.Domain.Models;

public class TechnicalPassport
{
    public Guid Id { get; set; }
    public string Number { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string FirstNameLatin { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string LastNameLatin { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string Address { get; set; } = null!;
    public DateOnly IssueDate { get; set; }
    public string SAICode { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public uint CreationYear { get; set; }
    public string Color { get; set; } = null!;
    public string VIN { get; set; } = null!;
    public string VehicleType { get; set; } = null!;
    public uint MaxWeight { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
}
