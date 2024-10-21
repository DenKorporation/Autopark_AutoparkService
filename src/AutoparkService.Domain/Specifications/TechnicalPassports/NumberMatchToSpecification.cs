using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class NumberMatchToSpecification : Specification<TechnicalPassport>
{
    public NumberMatchToSpecification(string number)
    {
        Criteria = x => x.Number.ToLower().Contains(number.ToLower());
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
