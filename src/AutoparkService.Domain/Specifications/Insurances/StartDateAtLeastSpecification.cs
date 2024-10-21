using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Insurances;

public class StartDateAtLeastSpecification : Specification<Insurance>
{
    public StartDateAtLeastSpecification(DateOnly date)
    {
        Criteria = x => x.StartDate >= date;
    }

    public override Expression<Func<Insurance, bool>> Criteria { get; }
}
