using System.Collections.Generic;
using System.Threading.Tasks;
using Better_Read_Telegram.TelegramBot.Models.Commands;
using Telegram.Bot;
using Command = Better_Read_Telegram.FunctionApp.BotSettings.Models.Commands.Command;
using StartCommand = Better_Read_Telegram.FunctionApp.BotSettings.Models.Commands.StartCommand;

namespace Better_Read_Telegram.TelegramBot.Models.Bots
{
    public static class BaseBot
    {
        private static TelegramBotClient botClient;
        private static List<Command> commandsList;

        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            commandsList = new List<Command>();
            commandsList.Add(new StartCommand());

            botClient = new TelegramBotClient(FunctionApp.BotSettings.AppSettings.Key);
            
            string hook = string.Format(FunctionApp.BotSettings.AppSettings.Url,"api/QueueTrigger"); 
            await botClient.SetWebhookAsync(hook);

            return botClient;
        }
    }
}