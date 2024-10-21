using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Permissions;

public class VehicleIdEqualToSpecification : Specification<Permission>
{
    public VehicleIdEqualToSpecification(Guid vehicleId)
    {
        Criteria = x => x.VehicleId == vehicleId;
    }

    public override Expression<Func<Permission, bool>> Criteria { get; }
}
