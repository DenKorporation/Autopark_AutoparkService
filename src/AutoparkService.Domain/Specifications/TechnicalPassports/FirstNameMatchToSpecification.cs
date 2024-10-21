using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class FirstNameMatchToSpecification : Specification<TechnicalPassport>
{
    public FirstNameMatchToSpecification(string firstName)
    {
        Criteria = x => x.FirstNameLatin.ToLower().Contains(firstName.ToLower());
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
