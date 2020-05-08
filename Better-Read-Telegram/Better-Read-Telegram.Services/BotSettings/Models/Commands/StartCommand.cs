using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Better_Read_Telegram.Services.BotSettings.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "/start";

        public override async Task CallBack(CallbackQuery callbackQuery, TelegramBotClient botClient)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Received {callbackQuery.Data}"
            );

            await botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"Received {callbackQuery.Data}"
            );
        }
        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.TextMessage)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Hello i'm LoveRead bot.\n Just write id of book from loveread.ec in format : [http://loveread.ec/view_global.php?id=#####] and i'll give you information about book. And you'll be able to download this book", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}