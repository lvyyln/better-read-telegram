using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BetterRead.OLD.BotSettings.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "/start";

        public override Task CallBack(CallbackQuery callbackQuery, TelegramBotClient botClient)
        {
            throw new NotImplementedException();
        }
        public override bool Contains(Message message) => 
            message.Type == MessageType.TextMessage && message.Text.Contains(Name);

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Hello i'm LoveRead bot.\n Just write id of book from loveread.ec in format : [http://loveread.ec/view_global.php?id=#####] and i'll give you information about book. And you'll be able to download this book", parseMode: ParseMode.Markdown);
        }
    }
}