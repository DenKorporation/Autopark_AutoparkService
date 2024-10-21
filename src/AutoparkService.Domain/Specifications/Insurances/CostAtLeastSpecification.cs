using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Insurances;

public class CostAtLeastSpecification : Specification<Insurance>
{
    public CostAtLeastSpecification(decimal cost)
    {
        Criteria = x => x.Cost >= cost;
    }

    public override Expression<Func<Insurance, bool>> Criteria { get; }
}
