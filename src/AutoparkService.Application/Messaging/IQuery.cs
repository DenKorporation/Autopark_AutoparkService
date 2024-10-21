using FluentResults;
using MediatR;

namespace AutoparkService.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
