using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class MaxWeightAtMostSpecification : Specification<TechnicalPassport>
{
    public MaxWeightAtMostSpecification(uint maxWeight)
    {
        Criteria = x => x.MaxWeight <= maxWeight;
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
