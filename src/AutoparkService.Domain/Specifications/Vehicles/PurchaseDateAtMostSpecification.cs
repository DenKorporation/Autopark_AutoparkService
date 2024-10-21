using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Vehicles;

public class PurchaseDateAtMostSpecification : Specification<Vehicle>
{
    public PurchaseDateAtMostSpecification(DateOnly date)
    {
        Criteria = x => x.PurchaseDate <= date;
    }

    public override Expression<Func<Vehicle, bool>> Criteria { get; }
}
