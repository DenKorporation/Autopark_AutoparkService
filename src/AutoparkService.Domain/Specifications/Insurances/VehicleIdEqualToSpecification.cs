using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Insurances;

public class VehicleIdEqualToSpecification : Specification<Insurance>
{
    public VehicleIdEqualToSpecification(Guid vehicleId)
    {
        Criteria = x => x.VehicleId == vehicleId;
    }

    public override Expression<Func<Insurance, bool>> Criteria { get; }
}
