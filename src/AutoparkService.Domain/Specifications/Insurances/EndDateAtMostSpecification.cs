using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Insurances;

public class EndDateAtMostSpecification : Specification<Insurance>
{
    public EndDateAtMostSpecification(DateOnly date)
    {
        Criteria = x => x.EndDate <= date;
    }

    public override Expression<Func<Insurance, bool>> Criteria { get; }
}
