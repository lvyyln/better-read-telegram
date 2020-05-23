using System.Threading;
using System.Threading.Tasks;
using BetterRead.Commands.Abstractions;
using BetterRead.MediatR.Core.HandlerResults.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace BetterRead.Commands.Start
{
    public class StartCommandHandler : CommandHandlerBase<StartCommand>
    {
        private readonly ITelegramBotClient _botClient;

        public StartCommandHandler(ITelegramBotClient botClient) => 
            _botClient = botClient;

        public override async Task<IHandlerResult> Handle(
            StartCommand request, 
            CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(request.ChatId,
                GetGreetingText("BetterRead"),
                parseMode: ParseMode.Markdown, 
                cancellationToken: cancellationToken);
            
            return Success();
        }

        private static string GetGreetingText(string botName) =>
            $"Hello i'm {botName} bot." +
            "\n" +
            "Just share here link to any book you like from http://loveread.ec";
    }
}