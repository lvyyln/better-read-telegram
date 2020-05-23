using System;
using System.IO;
using BetterRead.FunctionApp.Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;

namespace BetterRead.FunctionApp.Triggers.Abstractions
{
    public abstract class BaseTrigger
    {
        protected readonly ITelegramBotClient BotClient;
        protected readonly ICommandMatcher CommandMatcher;
        protected readonly ILogger Logger;

        protected BaseTrigger(
            ITelegramBotClient botClient, 
            ICommandMatcher commandMatcher, 
            ILogger logger) => 
            (BotClient, CommandMatcher, Logger) = (botClient, commandMatcher, logger);

        protected bool TryGetDataFromStream<T>(Stream stream, out T result) 
            where T : class
        {
            try
            {
                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Error occured while deserialize.");
                result = null;
                return false;
            }
        }
    }
}