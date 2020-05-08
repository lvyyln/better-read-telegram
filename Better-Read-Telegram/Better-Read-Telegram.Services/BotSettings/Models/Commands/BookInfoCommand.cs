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

namespace Better_Read_Telegram.Services.BotSettings.Models.Commands
{
    public class BookInfoCommand : Command
    {
        public BookInfoCommand()
        {
        }

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


        public static string Utf16ToUtf8(string utf16String)
        {
            // Get UTF16 bytes and convert UTF16 bytes to UTF8 bytes
            byte[] utf16Bytes = Encoding.Unicode.GetBytes(utf16String);
            byte[] utf8Bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf16Bytes);

            // Return UTF8 bytes as ANSI string
            return Encoding.Default.GetString(utf8Bytes);
        }
        
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

            return ConvertToFileToSend(bytes, "Image.jpg");
        }

        private FileToSend ConvertToFileToSend(byte[] bytes, string name)
            => new FileToSend(name, new MemoryStream(bytes));


        private string GenerateDataAboutBook(BookInfo bookInfo)
        {
            return " Імя автора: " + bookInfo.Author +
                   "\n Назва книги: " + bookInfo.Name;
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