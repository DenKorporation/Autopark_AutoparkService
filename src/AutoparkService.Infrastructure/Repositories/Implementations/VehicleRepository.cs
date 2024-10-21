using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Infrastructure.Contexts;

namespace AutoparkService.Infrastructure.Repositories.Implementations;

public class VehicleRepository(AutoParkDbContext dbContext)
    : Repository<Vehicle>(dbContext), IVehicleRepository
{
    private readonly AutoParkDbContext _dbContext = dbContext;

    public async Task LoadRelatedData(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        await _dbContext.Entry(vehicle).Reference(u => u.Permission).LoadAsync(cancellationToken);
        await _dbContext.Entry(vehicle).Reference(u => u.TechnicalPassport).LoadAsync(cancellationToken);
        await _dbContext.Entry(vehicle).Collection(u => u.Insurances).LoadAsync(cancellationToken);
        await _dbContext.Entry(vehicle).Collection(u => u.MaintenanceRecords).LoadAsync(cancellationToken);
    }
}
