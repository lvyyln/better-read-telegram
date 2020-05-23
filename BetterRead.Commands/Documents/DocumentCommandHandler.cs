using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BetterRead.Commands.Abstractions;
using BetterRead.Commands.Error;
using BetterRead.MediatR.Core.HandlerResults.Abstractions;
using BetterRead.Shared;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BetterRead.Commands.Documents
{
    public class DocumentCommandHandler : CommandHandlerBase<DocumentCommand>
    {
        private readonly ILoveRead _loveRead;
        private readonly ITelegramBotClient _botClient;

        public DocumentCommandHandler(ILoveRead loveRead, ITelegramBotClient botClient) => 
            (_loveRead, _botClient) = (loveRead, botClient);

        public override async Task<IHandlerResult> Handle(
            DocumentCommand request, 
            CancellationToken cancellationToken)
        {
            var read = await _botClient.SendTextMessageAsync(
                request.ChatId,
                "Reading...",
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);

            var book = await _loveRead.GetBookAsync(request.BookId);
            
            var generate = await _botClient.EditMessageTextAsync(
                request.ChatId,
                read.MessageId,
                "Generating document...",
                ParseMode.Markdown,
                cancellationToken: cancellationToken);
            
            var doc = await _loveRead.GetBookDocument(book);

            await _botClient.EditMessageTextAsync(
                request.ChatId,
                generate.MessageId,
                "Awesome reading!",
                ParseMode.Markdown,
                cancellationToken: cancellationToken);
            
            await _botClient.SendDocumentAsync(
                request.ChatId,
                new FileToSend("Document", new MemoryStream(doc)), 
                cancellationToken: cancellationToken);
            
            return Success();
        }
    }
}