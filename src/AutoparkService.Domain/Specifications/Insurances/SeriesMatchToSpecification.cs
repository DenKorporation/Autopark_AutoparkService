using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Insurances;

public class SeriesMatchToSpecification : Specification<Insurance>
{
    public SeriesMatchToSpecification(string series)
    {
        Criteria = x => x.Series.ToLower().Contains(series.ToLower());
    }

    public override Expression<Func<Insurance, bool>> Criteria { get; }
}
