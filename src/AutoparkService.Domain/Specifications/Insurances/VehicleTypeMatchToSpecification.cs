using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Insurances;

public class VehicleTypeMatchToSpecification : Specification<Insurance>
{
    public VehicleTypeMatchToSpecification(string vehicleType)
    {
        Criteria = x => x.VehicleType.ToLower().Contains(vehicleType.ToLower());
    }

    public override Expression<Func<Insurance, bool>> Criteria { get; }
}
