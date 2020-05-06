using Better_Read_Telegram.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;

namespace Better_Read_Telegram.FunctionApp.DependencyInjection
{
     public static class DependencyRegistrar
     {
         /// <summary>
         /// All dependencies should be resolved here
         /// </summary>
         /// <param name="builder">IWebJobsBuilder</param>
         /// <returns>IServiceCollection</returns>
         public static IServiceCollection RegisterDependencies(this IWebJobsBuilder builder) =>
             builder.Services
                 .AddTransient<IBookService, BookService>()
                 .AddTransient<IBotsService, BotsService>();
     }
}