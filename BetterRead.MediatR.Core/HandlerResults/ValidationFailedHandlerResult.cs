using BetterRead.MediatR.Core.HandlerResults.Abstractions;

namespace BetterRead.MediatR.Core.HandlerResults
{
    public class ValidationFailedHandlerResult : IHandlerResult
    {
        public ValidationFailedHandlerResult(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}