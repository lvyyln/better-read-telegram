using System.Reflection;
using AzureFunctions.Extensions.Swashbuckle;
using Better_Read_Telegram.FunctionApp;
using Better_Read_Telegram.TelegramBot.Models.Bots;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Better_Read_Telegram.FunctionApp
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            BaseBot.GetBotClientAsync().Wait();
        }
    }
}