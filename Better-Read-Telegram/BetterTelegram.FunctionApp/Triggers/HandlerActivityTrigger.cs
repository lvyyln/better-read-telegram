using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Better_Read_Telegram.FunctionApp.Triggers;
using Better_Read_Telegram.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Company.FunctionApp
{
    public class HandlerActivityTrigger : BaseTrigger
    {
        private readonly IBotsService _botsService;
        public HandlerActivityTrigger(IBotsService botsService)
        {
            _botsService = botsService;
        }
        [FunctionName("QueueTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequest request)
        {

            if (!TryGetDataFromStream(request.Body, out Update update))
                return new BadRequestResult();
            
            _botsService.HandleCommand(update);
            
            return new OkObjectResult("data");
        }
    } 
}