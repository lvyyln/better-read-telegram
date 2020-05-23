using System.Threading;
using System.Threading.Tasks;
using BetterRead.Commands.Abstractions;
using BetterRead.Commands.Error;
using BetterRead.MediatR.Core.HandlerResults;
using BetterRead.MediatR.Core.HandlerResults.Abstractions;
using BetterRead.Shared;
using Telegram.Bot;

namespace BetterRead.Commands.Validation
{
    public class ValidationLinkCommandHandler : CommandHandlerBase<ValidationLinkCommand>
    {
        private readonly ILoveRead _loveRead;
        private readonly ITelegramBotClient _botClient;

        public ValidationLinkCommandHandler(ILoveRead loveRead, ITelegramBotClient botClient) => 
            (_loveRead, _botClient) = (loveRead, botClient);

        public override async Task<IHandlerResult> Handle(
            ValidationLinkCommand request, 
            CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(request.ChatId, "Your link is invalid, please follow the example 'http://loveread.ec/view_global.php?id='", cancellationToken: cancellationToken);
            return new ValidationFailedHandlerResult("Please follow the example \n    'http://loveread.ec/view_global.php?id='");
        }
    }
}