using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Insurances;

public class NumberMatchToSpecification : Specification<Insurance>
{
    public NumberMatchToSpecification(string number)
    {
        Criteria = x => x.Number.ToLower().Contains(number.ToLower());
    }

    public override Expression<Func<Insurance, bool>> Criteria { get; }
}
