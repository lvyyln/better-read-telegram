using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Better_Read_Telegram.Services;
using BetterRead.Shared;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Better_Read_Telegram.FunctionApp.BotSettings.Models.Commands
{
    public class BookInfoCommand : Command
    {
        public BookInfoCommand()
        {
            
        }
        public override string Name => "/getBookInfo";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.TextMessage)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            var bookService = new LoveRead();
            var bookInfo = await bookService.GetBookInfoAsync("http://loveread.ec/view_global.php?id=45105");

            await botClient.SendPhoto(chatId, DownloadFile(bookInfo.ImageUrl));
            await botClient.SendTextMessage(chatId, $"Author : {bookInfo.Author}, Book Title: {bookInfo.Name}, BookUrl : {bookInfo.Url}",
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
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
            return new FileToSend("File.jpg",stream);
        }
    }
}