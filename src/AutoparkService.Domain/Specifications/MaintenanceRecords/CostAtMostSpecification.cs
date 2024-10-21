using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.MaintenanceRecords;

public class CostAtMostSpecification : Specification<MaintenanceRecord>
{
    public CostAtMostSpecification(decimal cost)
    {
        Criteria = x => x.Cost <= cost;
    }

    public override Expression<Func<MaintenanceRecord, bool>> Criteria { get; }
}
