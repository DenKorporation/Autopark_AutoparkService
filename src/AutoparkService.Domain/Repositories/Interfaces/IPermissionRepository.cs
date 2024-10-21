using AutoparkService.Domain.Models;

namespace AutoparkService.Domain.Repositories.Interfaces;

public interface IPermissionRepository : IRepository<Permission>
{
    public Task<Permission?> GetByNumberAsync(string number, CancellationToken cancellationToken = default);
    public Task<Permission?> GetByVehicleId(Guid vehicleId, CancellationToken cancellationToken = default);
}
