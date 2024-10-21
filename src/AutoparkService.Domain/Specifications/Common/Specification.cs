using System.Linq.Expressions;

namespace AutoparkService.Domain.Specifications.Common;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> Criteria { get; }

    public bool IsSatisfiedBy(T entity)
    {
        Func<T, bool> predicate = Criteria.Compile();

        return predicate(entity);
    }

    public Specification<T> And(Specification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }
}
