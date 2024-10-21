using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class VINMatchToSpecification : Specification<TechnicalPassport>
{
    public VINMatchToSpecification(string vin)
    {
        Criteria = x => x.VIN.ToLower().Contains(vin.ToLower());
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
