using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BetterRead.Shared;
using BetterRead.Shared.Domain.Books;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Better_Read_Telegram.Services.BotSettings.Models.Commands
{
    public class BookInfoCommand : Command
    {
        public BookInfoCommand()
        {
        }

        static InlineKeyboardMarkup _myInlineKeyboard;

        public LoveRead LoveRead => new LoveRead();
        public override string Name => "http://loveread.ec/view_global.php?id=";

        public override async Task CallBack(CallbackQuery callbackQuery, TelegramBotClient botClient)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Received {callbackQuery.Data}"
            );

            var t = await GetBookDocumentAsync(callbackQuery);

            await botClient.SendDocumentAsync(
                callbackQuery.Message.Chat.Id,
                ConvertToFileToSend(t)
            );
        }

        public override bool Contains(Message message) =>
            Int32.TryParse(message.Text, out _);


        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            MapKeyBoard();
            var chatId = message.Chat.Id;


            var bookInfo = await LoveRead.GetBookInfoAsync(Name + message.Text);

            await botClient.SendPhotoAsync(chatId, DownloadFile(bookInfo.ImageUrl));
            await botClient.SendTextMessageAsync(chatId, GenerateDataAboutBook(bookInfo),
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            await botClient.SendTextMessageAsync(chatId, $"Ссилка на книгу: {bookInfo.Url}",
                replyMarkup: _myInlineKeyboard);
        }


        private async Task<byte[]> GetBookDocumentAsync(CallbackQuery callbackQuery)
        {
            return await LoveRead.GetBookDocument(
                await LoveRead.GetBookAsync(callbackQuery.Message.Text.Split(":")[1]));
        }

        private FileToSend DownloadFile(string fromUrl, byte[] bytes = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (WebClient client = new WebClient())
                {
                    bytes = client.DownloadData(new Uri(fromUrl));
                }

                var tw = new StreamWriter(ms);
                tw.Write(bytes);
                tw.Flush();
            }

            return ConvertToFileToSend(bytes);
        }

        private FileToSend ConvertToFileToSend(byte[] bytes) 
            => new FileToSend("File", new MemoryStream(bytes));


        private string GenerateDataAboutBook(BookInfo bookInfo)
        {
            return " Імя автора: " + bookInfo.Author +
                   " Назва книги: " + bookInfo.Name;
        }

        private void MapKeyBoard()
        {
            _myInlineKeyboard = new InlineKeyboardMarkup()
            {
                InlineKeyboard = new InlineKeyboardButton[][]
                {
                    new InlineKeyboardButton[] //first row
                    {
                        new InlineKeyboardButton("Download Document", "CallbackQuery1")
                    }
                }
            };
        }
    }
}