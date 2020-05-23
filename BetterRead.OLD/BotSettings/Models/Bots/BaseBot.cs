using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetterRead.OLD.BotSettings.Models.Commands;
using Telegram.Bot;
using Command = BetterRead.OLD.BotSettings.Models.Commands.Command;
using StartCommand = BetterRead.OLD.BotSettings.Models.Commands.StartCommand;

namespace BetterRead.OLD.BotSettings.Models.Bots
{
    public static class BaseBot
    {
        private static TelegramBotClient _botClient;
        
        private static List<Command> _commandsList;
        public static IEnumerable<Command> Commands => 
            _commandsList;

        public static Command Callback { get; private set; }

        public static Task<TelegramBotClient> GetBotClientAsync()
        {
            _commandsList = new List<Command> {new StartCommand(), new BookInfoCommand()};
            Callback = new BookInfoCommand();
            
            throw new NotImplementedException();
        }
    }
}