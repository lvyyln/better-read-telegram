using System;
using System.Threading.Tasks;
using BetterRead.FunctionApp.Infrastructure.Abstractions;
using BetterRead.FunctionApp.Triggers.Abstractions;
using BetterRead.MediatR.Core.HandlerResults;
using BetterRead.MediatR.Core.HandlerResults.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BetterRead.FunctionApp.Triggers
{
    public class UpdateTrigger : BaseTrigger, IDisposable
    {
        private readonly IMediator _mediator;
        
        public UpdateTrigger(
            ICommandMatcher commandMatcher, 
            ILogger logger, 
            ITelegramBotClient botClient, 
            IMediator mediator)
            : base(botClient, commandMatcher, logger) =>
            _mediator = mediator;

        public void Dispose() => 
            Logger.LogInformation($"Disposing {this}");

        [FunctionName(nameof(Update))]
        public async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequest request)
        {
            if (!TryGetDataFromStream(request.Body, out Update update))
                return new OkResult();

            var command = CommandMatcher.MatchCommandAsync(update);
            return await _mediator.Send(command) switch
            {
                NotFoundHandlerResult _ => new NotFoundResult(),
                SuccessHandlerResult _ => new OkResult(),
                ValidationFailedHandlerResult res => new OkObjectResult(res),
                _ => throw new ArgumentOutOfRangeException(typeof(IHandlerResult).ToString())
            };
        }
    }
}