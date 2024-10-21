using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class CreationYearAtLeastSpecification : Specification<TechnicalPassport>
{
    public CreationYearAtLeastSpecification(uint creationYear)
    {
        Criteria = x => x.CreationYear >= creationYear;
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
