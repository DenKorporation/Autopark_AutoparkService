using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.MaintenanceRecords;

public class ServiceCenterMatchToSpecification : Specification<MaintenanceRecord>
{
    public ServiceCenterMatchToSpecification(string serviceCenter)
    {
        Criteria = x => x.ServiceCenter.ToLower().Contains(serviceCenter.ToLower());
    }

    public override Expression<Func<MaintenanceRecord, bool>> Criteria { get; }
}
