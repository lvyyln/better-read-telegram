using System.Reflection;
using AzureFunctions.Extensions.Swashbuckle;
using Better_Read_Telegram.FunctionApp;
using Better_Read_Telegram.FunctionApp.BotSettings.Models.Bots;
using Better_Read_Telegram.FunctionApp.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Better_Read_Telegram.FunctionApp
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.RegisterDependencies();
            BaseBot.GetBotClientAsync().Wait();
        }
    }
}