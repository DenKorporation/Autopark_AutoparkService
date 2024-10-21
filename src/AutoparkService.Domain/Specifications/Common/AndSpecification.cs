using System.Linq.Expressions;

namespace AutoparkService.Domain.Specifications.Common;

public class AndSpecification<T> : Specification<T>
{
    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        Expression<Func<T, bool>> leftExpression = left.Criteria;
        Expression<Func<T, bool>> rightExpression = right.Criteria;

        BinaryExpression andExpression = Expression.AndAlso(
            leftExpression.Body,
            rightExpression.Body);

        Criteria = Expression.Lambda<Func<T, bool>>(
            andExpression,
            leftExpression.Parameters.Single());
    }

    public override Expression<Func<T, bool>> Criteria { get; }
}
