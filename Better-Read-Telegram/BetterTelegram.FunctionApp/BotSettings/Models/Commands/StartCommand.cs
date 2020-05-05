using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Better_Read_Telegram.FunctionApp.BotSettings.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "/start";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.TextMessage)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Hello i'm ", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}