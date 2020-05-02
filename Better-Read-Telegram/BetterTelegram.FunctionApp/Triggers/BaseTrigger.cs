using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Better_Read_Telegram.FunctionApp.Triggers
{
    /// <summary>
    /// BaseFunction
    /// </summary>
    public abstract class BaseTrigger
    {
        /// <summary>
        /// Run function body and try to catch the exceptions
        /// </summary>
        /// <param name="funcBody">Main function calls.</param>
        /// <returns>API Action Result</returns>
        protected static async Task<IActionResult> TryInvokeFunc(Func<Task<IActionResult>> funcBody)
        {
            try
            {
                return await funcBody.Invoke();
            }
            catch (Exception exception)
            {
                Debugger.Log((int) LogLevel.Error, "Exception", $"{exception.Message}\n{exception.StackTrace}");
                return new BadRequestErrorMessageResult(
                    $"Oops... Unhandled exception! Message: {exception.Message}");
            }
        }

        /// <summary>
        /// Generic way to convert Function HttpRequest into DTO object.
        /// </summary>
        /// <param name="stream">HttpRequest.Body parameter from request.</param>
        /// <param name="result">Converted request body to specified output type.</param>
        /// <typeparam name="T">Output data type.</typeparam>
        /// <returns>FALSE if it's not possible to deserialize stream to output type.</returns>
        protected static bool TryGetDataFromStream<T>(Stream stream, out T result) 
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
                result = null;
                return false;
            }
        }
        
        /// <summary>
        /// Get current version from Assembly Info properties.
        /// </summary>
        protected static string CurrentFunctionVersion =>
            Assembly.GetEntryAssembly()?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? "unknown";
    }
}