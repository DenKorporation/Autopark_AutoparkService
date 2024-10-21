using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.Insurances;

public class ProviderMatchToSpecification : Specification<Insurance>
{
    public ProviderMatchToSpecification(string provider)
    {
        Criteria = x => x.Provider.ToLower().Contains(provider.ToLower());
    }

    public override Expression<Func<Insurance, bool>> Criteria { get; }
}
