using System.Threading;
using System.Threading.Tasks;
using BetterRead.MediatR.Core.HandlerResults;
using BetterRead.MediatR.Core.HandlerResults.Abstractions;
using MediatR;

namespace BetterRead.MediatR.Core
{
    public abstract class RequestHandlerBase<TRequest> : IRequestHandler<TRequest, IHandlerResult>
        where TRequest : IRequest<IHandlerResult>
    {
        public abstract Task<IHandlerResult> Handle(TRequest request, CancellationToken cancellationToken);

        protected static IHandlerResult Success() =>
            new SuccessHandlerResult();

        protected static IHandlerResult NotFound() =>
            new NotFoundHandlerResult();

        protected static IHandlerResult ValidationFailed(string message) =>
            new ValidationFailedHandlerResult(message);
    }
}