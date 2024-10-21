using FluentResults;
using MediatR;

namespace AutoparkService.Application.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

public interface ICommand : IRequest<Result>
{
}
