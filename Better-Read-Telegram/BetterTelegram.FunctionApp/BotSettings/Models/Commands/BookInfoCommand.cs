using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Better_Read_Telegram.Services;
using BetterRead.Shared;
using BetterRead.Shared.Domain.Books;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Better_Read_Telegram.FunctionApp.BotSettings.Models.Commands
{
    public class BookInfoCommand : Command
    {
        public BookInfoCommand()
        {
        }

        static InlineKeyboardMarkup _myInlineKeyboard;

        public override string Name => "http://loveread.ec/view_global.php?id=";

        public override bool Contains(Message message) =>
            Int32.TryParse(message.Text, out _);

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            MapKeyBoard();
            var chatId = message.Chat.Id;

            var bookService = new LoveRead();
            var bookInfo = await bookService.GetBookInfoAsync(Name + message.Text);

            await botClient.SendPhotoAsync(chatId, DownloadFile(bookInfo.ImageUrl));
            await botClient.SendTextMessageAsync(chatId, GenerateDataAboutBook(bookInfo),
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            await botClient.SendTextMessageAsync(chatId, "Your_Text", replyMarkup: _myInlineKeyboard);
        }

        private FileToSend DownloadFile(string fromUrl)
        {
            byte[] bytes = null;
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

            var stream = new MemoryStream(bytes);
            return new FileToSend("File.jpg", stream);
        }

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
                        new InlineKeyboardButton("option1", "CallbackQuery1"), //first column
                        new InlineKeyboardButton("option2", "CallbackQuery2") //second column
                    }
                }
            };
        }
    }
}