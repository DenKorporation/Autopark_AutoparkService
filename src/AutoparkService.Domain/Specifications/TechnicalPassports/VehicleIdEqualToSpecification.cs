using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class VehicleIdEqualToSpecification : Specification<TechnicalPassport>
{
    public VehicleIdEqualToSpecification(Guid vehicleId)
    {
        Criteria = x => x.VehicleId == vehicleId;
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
