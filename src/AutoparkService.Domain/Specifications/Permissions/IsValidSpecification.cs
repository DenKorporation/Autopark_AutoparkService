using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Permissions;

public class IsValidSpecification : Specification<Permission>
{
    public IsValidSpecification(bool isValid)
    {
        Criteria = x => x.ExpiryDate >= DateOnly.FromDateTime(DateTime.Today) == isValid;
    }

    public override Expression<Func<Permission, bool>> Criteria { get; }
}
