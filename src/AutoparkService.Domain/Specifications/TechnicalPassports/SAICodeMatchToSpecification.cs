using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class SAICodeMatchToSpecification : Specification<TechnicalPassport>
{
    public SAICodeMatchToSpecification(string code)
    {
        Criteria = x => x.SAICode.ToLower().Contains(code.ToLower());
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
