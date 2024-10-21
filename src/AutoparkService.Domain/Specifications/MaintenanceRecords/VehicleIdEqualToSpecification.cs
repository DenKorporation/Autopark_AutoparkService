using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.MaintenanceRecords;

public class VehicleIdEqualToSpecification : Specification<MaintenanceRecord>
{
    public VehicleIdEqualToSpecification(Guid vehicleId)
    {
        Criteria = x => x.VehicleId == vehicleId;
    }

    public override Expression<Func<MaintenanceRecord, bool>> Criteria { get; }
}
