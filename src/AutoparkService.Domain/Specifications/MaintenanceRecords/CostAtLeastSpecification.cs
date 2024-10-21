using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.MaintenanceRecords;

public class CostAtLeastSpecification : Specification<MaintenanceRecord>
{
    public CostAtLeastSpecification(decimal cost)
    {
        Criteria = x => x.Cost >= cost;
    }

    public override Expression<Func<MaintenanceRecord, bool>> Criteria { get; }
}
