using AutoparkService.Domain.Models;

namespace AutoparkService.Domain.Repositories.Interfaces;

public interface ITechnicalPassportRepository : IRepository<TechnicalPassport>
{
    public Task<TechnicalPassport?> GetByNumberAsync(
        string number,
        CancellationToken cancellationToken = default);

    public Task<TechnicalPassport?> GetByVinAsync(
        string vin,
        CancellationToken cancellationToken = default);

    public Task<TechnicalPassport?> GetByLicensePlateAsync(
        string licensePlate,
        CancellationToken cancellationToken = default);

    public Task<TechnicalPassport?> GetByVehicleId(
        Guid vehicleId,
        CancellationToken cancellationToken = default);
}
