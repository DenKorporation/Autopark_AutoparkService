using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.MaintenanceRecords;

public class EndDateAtMostSpecification : Specification<MaintenanceRecord>
{
    public EndDateAtMostSpecification(DateOnly date)
    {
        Criteria = x => x.EndDate <= date;
    }

    public override Expression<Func<MaintenanceRecord, bool>> Criteria { get; }
}
