using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.MaintenanceRecords;

public class TypeMatchToSpecification : Specification<MaintenanceRecord>
{
    public TypeMatchToSpecification(string type)
    {
        Criteria = x => x.Type.ToLower().Contains(type.ToLower());
    }

    public override Expression<Func<MaintenanceRecord, bool>> Criteria { get; }
}
