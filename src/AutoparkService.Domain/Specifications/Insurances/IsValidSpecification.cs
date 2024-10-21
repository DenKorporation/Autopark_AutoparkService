using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Insurances;

public class IsValidSpecification : Specification<Insurance>
{
    public IsValidSpecification(bool isValid)
    {
        Criteria = x => x.EndDate >= DateOnly.FromDateTime(DateTime.Today) == isValid;
    }

    public override Expression<Func<Insurance, bool>> Criteria { get; }
}
