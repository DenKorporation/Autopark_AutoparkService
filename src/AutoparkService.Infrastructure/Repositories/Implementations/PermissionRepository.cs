using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AutoparkService.Infrastructure.Repositories.Implementations;

public class PermissionRepository(AutoParkDbContext dbContext)
    : Repository<Permission>(dbContext), IPermissionRepository
{
    private readonly AutoParkDbContext _dbContext = dbContext;

    public async Task<Permission?> GetByNumberAsync(string number, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Permissions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Number == number, cancellationToken);
    }

    public async Task<Permission?> GetByVehicleId(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Permissions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.VehicleId == vehicleId, cancellationToken);
    }
}
