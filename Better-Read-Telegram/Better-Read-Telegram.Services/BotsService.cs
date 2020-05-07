using Better_Read_Telegram.Services.BotSettings.Models.Bots;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Better_Read_Telegram.Services
{
    public interface IBotsService
    {
        void HandleCommand(Update update);
    }

    public class BotsService : IBotsService
    {
        public async void HandleCommand(Update update)
        {
            var commands = BaseBot.Commands;
            var callbacks = BaseBot.Callabck;
            var botClient = await BaseBot.GetBotClientAsync();
            var message = update.Message;
            var callback = update.CallbackQuery;
            
            if (update.Type == UpdateType.CallbackQueryUpdate)
            {
                await callbacks.CallBack(callback, botClient);
                return;
            }
            
            foreach (var command in commands)
            {
                if (!command.Contains(message)) continue;
                await command.Execute(message, botClient);
                break;
            }
        }
    }
}