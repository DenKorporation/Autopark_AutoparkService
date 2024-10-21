using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AutoparkService.Infrastructure.Repositories.Implementations;

public class TechnicalPassportRepository(AutoParkDbContext dbContext)
    : Repository<TechnicalPassport>(dbContext), ITechnicalPassportRepository
{
    private readonly AutoParkDbContext _dbContext = dbContext;

    public async Task<TechnicalPassport?> GetByNumberAsync(string number, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .TechnicalPassports
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Number == number, cancellationToken);
    }

    public async Task<TechnicalPassport?> GetByVinAsync(string vin, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .TechnicalPassports
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.VIN == vin, cancellationToken);
    }

    public async Task<TechnicalPassport?> GetByLicensePlateAsync(
        string licensePlate,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .TechnicalPassports
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.LicensePlate == licensePlate, cancellationToken);
    }

    public async Task<TechnicalPassport?> GetByVehicleId(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .TechnicalPassports
            .AsNoTracking()
            .FirstOrDefaultAsync(tp => tp.VehicleId == vehicleId, cancellationToken);
    }
}
