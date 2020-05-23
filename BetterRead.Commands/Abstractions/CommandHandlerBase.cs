using BetterRead.MediatR.Core;

namespace BetterRead.Commands.Abstractions
{
    public abstract class CommandHandlerBase<TRequest> : RequestHandlerBase<TRequest>
        where TRequest : ICommand
    {
    }
}