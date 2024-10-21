using AutoparkService.Domain.Models;

namespace AutoparkService.Domain.Repositories.Interfaces;

public interface IInsuranceRepository : IRepository<Insurance>
{
    public Task<Insurance?> GetBySeriesAndNumberAsync(
        string series,
        string number,
        CancellationToken cancellationToken = default);
}
