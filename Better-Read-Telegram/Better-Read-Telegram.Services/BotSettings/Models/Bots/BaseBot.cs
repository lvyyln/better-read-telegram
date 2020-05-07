using System.Collections.Generic;
using System.Threading.Tasks;
using Better_Read_Telegram.Services.BotSettings.Models.Commands;
using Telegram.Bot;
using Command = Better_Read_Telegram.Services.BotSettings.Models.Commands.Command;
using StartCommand = Better_Read_Telegram.Services.BotSettings.Models.Commands.StartCommand;

namespace Better_Read_Telegram.Services.BotSettings.Models.Bots
{
    public static class BaseBot
    {
        private static TelegramBotClient botClient;
        private static List<Command> commandsList;
        private static Command callback;

        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();
        public static Command Callabck => callback;

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            commandsList = new List<Command>();
            commandsList.Add(new StartCommand());
            commandsList.Add(new BookInfoCommand());
            callback = new BookInfoCommand();
            botClient = new TelegramBotClient(AppSettings.Key);
            
            string hook = string.Format(AppSettings.Url,"api/QueueTrigger"); 
            await botClient.SetWebhookAsync(hook);

            return botClient;
        }
    }
}