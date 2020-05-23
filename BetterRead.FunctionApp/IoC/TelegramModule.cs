using System;
using Autofac;
using BetterRead.Configuration;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace BetterRead.FunctionApp.IoC
{
    public class TelegramModule : Module
    {
        protected override void Load(ContainerBuilder builder) => 
            RegisterTelegram(builder);

        private static void RegisterTelegram(ContainerBuilder builder)
        {
            builder
                .Register(activator =>
                {
                    var botConfiguration = GetBotConfiguration(activator);
                    return GetTelegramBotClient(botConfiguration);
                })
                .As<ITelegramBotClient>()
                .SingleInstance();
        }

        private static BotConfiguration GetBotConfiguration(IComponentContext activator)
        {
            var botConfiguration = new BotConfiguration();
            var configuration = activator.Resolve<IConfiguration>();
            configuration.GetSection(nameof(BotConfiguration)).Bind(botConfiguration);
            return botConfiguration;
        }
        
        private static TelegramBotClient GetTelegramBotClient(BotConfiguration botConfiguration)
        {
            var client = new TelegramBotClient(botConfiguration.Key);
            client.SetWebhookAsync(botConfiguration.Url).ConfigureAwait(false).GetAwaiter().GetResult();
            return client;
        }
    }
}