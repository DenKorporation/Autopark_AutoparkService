using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Permissions;

public class ExpiryDateAtMostSpecification : Specification<Permission>
{
    public ExpiryDateAtMostSpecification(DateOnly date)
    {
        Criteria = x => x.ExpiryDate <= date;
    }

    public override Expression<Func<Permission, bool>> Criteria { get; }
}
