using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AutoparkService.Infrastructure.Repositories.Implementations;

public class InsuranceRepository(AutoParkDbContext dbContext)
    : Repository<Insurance>(dbContext), IInsuranceRepository
{
    private readonly AutoParkDbContext _dbContext = dbContext;

    public async Task<Insurance?> GetBySeriesAndNumberAsync(
        string series,
        string number,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Insurances
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Series == series && i.Number == number, cancellationToken);
    }
}
