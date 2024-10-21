using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Vehicles;

public class StatusEqualToSpecification : Specification<Vehicle>
{
    public StatusEqualToSpecification(string status)
    {
        Criteria = x => x.Status.ToLower().Equals(status.ToLower());
    }

    public override Expression<Func<Vehicle, bool>> Criteria { get; }
}
