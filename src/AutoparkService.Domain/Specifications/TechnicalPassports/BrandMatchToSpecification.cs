using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class BrandMatchToSpecification : Specification<TechnicalPassport>
{
    public BrandMatchToSpecification(string brand)
    {
        Criteria = x => x.Brand.ToLower().Contains(brand.ToLower());
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
