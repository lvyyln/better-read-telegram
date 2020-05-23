using BetterRead.MediatR.Core.HandlerResults.Abstractions;
using MediatR;

namespace BetterRead.Commands.Abstractions
{
    public interface ICommand : IRequest<IHandlerResult>
    {
    }
}