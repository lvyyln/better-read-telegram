using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Better_Read_Telegram.FunctionApp.BotSettings.Models.Bots;
using Better_Read_Telegram.FunctionApp.Triggers;
using Better_Read_Telegram.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Company.FunctionApp
{
    public class SimpleExample : BaseTrigger
    {

        [FunctionName("QueueTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequest request)
        {
            
            var commands = BaseBot.Commands;
            if (!TryGetDataFromStream(request.Body, out Update update))
                return new BadRequestResult();
            
            var message = update.Message;
            var botClient = await BaseBot.GetBotClientAsync();

            foreach (var command in commands)
            {
                if (command.Contains(message))
                {
                    await command.Execute(message, botClient);
                    break;
                }
            }
            return new OkObjectResult("data");
        }
    } 
}