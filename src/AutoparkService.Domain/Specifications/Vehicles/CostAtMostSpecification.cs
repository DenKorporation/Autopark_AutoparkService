using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Vehicles;

public class CostAtMostSpecification : Specification<Vehicle>
{
    public CostAtMostSpecification(decimal cost)
    {
        Criteria = x => x.Cost <= cost;
    }

    public override Expression<Func<Vehicle, bool>> Criteria { get; }
}
