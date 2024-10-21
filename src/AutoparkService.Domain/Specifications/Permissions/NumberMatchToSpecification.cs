using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Permissions;

public class NumberMatchToSpecification : Specification<Permission>
{
    public NumberMatchToSpecification(string number)
    {
        Criteria = x => x.Number.ToLower().Contains(number.ToLower());
    }

    public override Expression<Func<Permission, bool>> Criteria { get; }
}
