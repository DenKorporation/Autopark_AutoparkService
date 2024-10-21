using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class LicensePlateMatchToSpecification : Specification<TechnicalPassport>
{
    public LicensePlateMatchToSpecification(string licensePlate)
    {
        Criteria = x => x.LicensePlate.ToLower().Contains(licensePlate.ToLower());
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
