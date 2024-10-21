using System.Linq.Expressions;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Specifications.TechnicalPassports;

public class ModelMatchToSpecification : Specification<TechnicalPassport>
{
    public ModelMatchToSpecification(string model)
    {
        Criteria = x => x.Model.ToLower().Contains(model.ToLower());
    }

    public override Expression<Func<TechnicalPassport, bool>> Criteria { get; }
}
