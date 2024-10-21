using AutoparkService.Domain.Models;

namespace AutoparkService.Domain.Repositories.Interfaces;

public interface IVehicleRepository : IRepository<Vehicle>
{
    public Task LoadRelatedData(Vehicle vehicle, CancellationToken cancellationToken = default);
}
