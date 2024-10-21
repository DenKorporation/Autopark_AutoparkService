using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.MaintenanceRecords;

public class OdometerAtMostSpecification : Specification<MaintenanceRecord>
{
    public OdometerAtMostSpecification(int odometer)
    {
        Criteria = x => x.Odometer <= odometer;
    }

    public override Expression<Func<MaintenanceRecord, bool>> Criteria { get; }
}
