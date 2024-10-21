using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class LastNameMatchToSpecification : Specification<TechnicalPassport>
{
    public LastNameMatchToSpecification(string lastName)
    {
        Criteria = x => x.LastNameLatin.ToLower().Contains(lastName.ToLower());
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
