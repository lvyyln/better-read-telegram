using System.Threading.Tasks;
using BetterRead.FunctionApp.Infrastructure.Abstractions;
using BetterRead.FunctionApp.Triggers.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace BetterRead.FunctionApp.Triggers
{
    public class RegisterTrigger : BaseTrigger
    {
        public RegisterTrigger(
            ITelegramBotClient botClient, 
            ICommandMatcher commandMatcher, 
            ILogger logger) 
            : base(botClient, commandMatcher, logger)
        {
        }
        
        [FunctionName(nameof(Register))]
        public async Task<IActionResult> Register(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequest request)
        {
            var me = await BotClient.GetMeAsync();
            return new OkObjectResult(me.Username);
        }
    }
}