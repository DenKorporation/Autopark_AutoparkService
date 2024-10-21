using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Vehicles;

public class UserIdEqualToSpecification : Specification<Vehicle>
{
    public UserIdEqualToSpecification(Guid userId)
    {
        Criteria = x => x.UserId == userId;
    }

    public override Expression<Func<Vehicle, bool>> Criteria { get; }
}
