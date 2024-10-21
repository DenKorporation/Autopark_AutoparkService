using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Vehicles;

public class OdometerAtMostSpecification : Specification<Vehicle>
{
    public OdometerAtMostSpecification(int odometer)
    {
        Criteria = x => x.Odometer <= odometer;
    }

    public override Expression<Func<Vehicle, bool>> Criteria { get; }
}
