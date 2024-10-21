using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.MaintenanceRecords;

public class StartDateAtLeastSpecification : Specification<MaintenanceRecord>
{
    public StartDateAtLeastSpecification(DateOnly date)
    {
        Criteria = x => x.StartDate >= date;
    }

    public override Expression<Func<MaintenanceRecord, bool>> Criteria { get; }
}
