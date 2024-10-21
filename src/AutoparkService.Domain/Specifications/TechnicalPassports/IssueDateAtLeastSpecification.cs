using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class IssueDateAtLeastSpecification : Specification<TechnicalPassport>
{
    public IssueDateAtLeastSpecification(DateOnly date)
    {
        Criteria = x => x.IssueDate >= date;
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
