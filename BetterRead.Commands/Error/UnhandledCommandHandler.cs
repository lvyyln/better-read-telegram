using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BetterRead.Commands.Abstractions;
using BetterRead.Commands.Documents;
using BetterRead.MediatR.Core.HandlerResults;
using BetterRead.MediatR.Core.HandlerResults.Abstractions;
using BetterRead.Shared;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BetterRead.Commands.Error
{
    public class UnhandledCommandHandler
    {
        public class DocumentCommandHandler : CommandHandlerBase<UnhandledCommand>
        {
            private readonly ILoveRead _loveRead;
            private readonly ITelegramBotClient _botClient;

            public DocumentCommandHandler(ILoveRead loveRead, ITelegramBotClient botClient) => 
                (_loveRead, _botClient) = (loveRead, botClient);

            public override async Task<IHandlerResult> Handle(
                UnhandledCommand request, 
                CancellationToken cancellationToken)
            {
                await _botClient.SendTextMessageAsync(request.ChatId, "Sorry but we don't support this command", cancellationToken: cancellationToken);
                return new ValidationFailedHandlerResult("Sorry but we don't support this command");
            }
        }
    }
}