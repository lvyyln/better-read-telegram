using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BetterRead.Shared;
using BetterRead.Shared.Domain.Books;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BetterRead.OLD.BotSettings.Models.Commands
{
    public class BookInfoCommand : Command
    {
        static InlineKeyboardMarkup _myInlineKeyboard;

        public LoveRead LoveRead => new LoveRead();
        public static string _bookId;
        public override string Name => "http://loveread.ec/view_global.php?id=";

        public override async Task CallBack(CallbackQuery callbackQuery, TelegramBotClient botClient)
        {
            var book = await LoveRead.GetBookAsync(_bookId);
            var doc = await LoveRead.GetBookDocument(book);
            byte[] data = Encoding.UTF32.GetBytes("Привет");
            await botClient.SendDocumentAsync(
                callbackQuery.Message.Chat.Id,
                ConvertToFileToSend(doc,Encoding.Unicode.GetString(data))
            );

            await botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Документ відправлено"
            );
        }

        public override bool Contains(Message message) =>
            Uri.IsWellFormedUriString(message.Text, UriKind.Absolute) &&
            message.Text.StartsWith("http://loveread.ec/view_global.php?id=");

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            MapKeyBoard();
            var chatId = message.Chat.Id;

            _bookId = message.Text;
            var bookInfo = await LoveRead.GetBookInfoAsync(message.Text);

            await botClient.SendPhotoAsync(chatId, DownloadFile(bookInfo.ImageUrl));
            await botClient.SendTextMessageAsync(chatId, GenerateDataAboutBook(bookInfo),
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            await botClient.SendTextMessageAsync(chatId, "Скачати книгу:",
                replyMarkup: _myInlineKeyboard);
        }

        private static FileToSend DownloadFile(string fromUrl, byte[] bytes = null)
        {
            using (var ms = new MemoryStream())
            {
                using (var client = new WebClient())
                {
                    bytes = client.DownloadData(new Uri(fromUrl));
                }

                var tw = new StreamWriter(ms);
                tw.Write(bytes);
                tw.Flush();
            }

            return ConvertToFileToSend(bytes, "Image.jpg");
        }

        private static FileToSend ConvertToFileToSend(byte[] bytes, string name)
            => new FileToSend(name, new MemoryStream(bytes));
        
        private static string GenerateDataAboutBook(BookInfo bookInfo) =>
            "Імя автора: " + bookInfo.Author +
            "\nНазва книги: " + bookInfo.Name;

        private static void MapKeyBoard()
        {
            _myInlineKeyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new[]
                {
                    new[]
                    {
                        new InlineKeyboardButton("Download Document", "CallbackQuery1")
                    }
                }
            };
        }
    }
}