using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BetterRead.Commands.Abstractions;
using BetterRead.MediatR.Core.HandlerResults.Abstractions;
using BetterRead.Shared;
using BetterRead.Shared.Domain.Books;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BetterRead.Commands.Books
{
    public class BookInfoCommandHandler : CommandHandlerBase<BookInfoCommand>
    {
        private readonly Regex _regex = new Regex("http:\\/\\/loveread\\.ec\\/view_global\\.php\\?id=(?<!\\d)\\d{5}(?!\\d)");
        private readonly Predicate<string> _condition = s => !_regex.IsMatch(s);
        private readonly ILoveRead _loveRead;
        private readonly ITelegramBotClient _botClient;

        public BookInfoCommandHandler(ILoveRead loveRead, ITelegramBotClient botClient) => 
            (_loveRead, _botClient) = (loveRead,botClient);

        public override async Task<IHandlerResult> Handle(
            BookInfoCommand request,
            CancellationToken cancellationToken)
        {
            if (_condition(request.Url)) return ValidationFailed("Bad book url");

            var bookInfo = await _loveRead.GetBookInfoAsync(request.Url);
            
            await _botClient.SendPhotoAsync(
                request.ChatId,
                GetPhotoFile(bookInfo.Name, bookInfo.ImageUrl),
                cancellationToken: cancellationToken);
            
            await _botClient.SendTextMessageAsync(
                request.ChatId,
                GetText(bookInfo),
                replyMarkup: GetKeyBoard(bookInfo.BookId),
                cancellationToken: cancellationToken); 
            
            return Success();
        }

        private static FileToSend GetPhotoFile(string bookName, string coverUrl)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var client = new WebClient())
                {
                    bytes = client.DownloadData(new Uri(coverUrl));
                }

                var tw = new StreamWriter(ms);
                tw.Write(bytes);
                tw.Flush();
            }
            return new FileToSend($"{bookName}.jpg", new MemoryStream(bytes));
        }

        private static string GetText(BookInfo bookInfo) => 
            $"Author: {bookInfo.Author}\nBook: {bookInfo.Name}";

        private static InlineKeyboardMarkup GetKeyBoard(int bookId) =>
            new InlineKeyboardMarkup
            {
                InlineKeyboard = new[]
                {
                    new[]
                    {
                        new InlineKeyboardButton("Download", bookId.ToString())                       
                    }
                }
            };
    }
}