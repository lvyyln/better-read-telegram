using Autofac;
using Autofac.Extensions.DependencyInjection.AzureFunctions;
using AutoMapper;
using BetterRead.FunctionApp;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace BetterRead.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.UseAppSettings();
            
            builder
                .Services
                .AddAutoMapper(typeof(Startup).Assembly);
            
            builder.UseAutofacServiceProviderFactory(ConfigureContainer);
        }

        private static void ConfigureContainer(ContainerBuilder builder) => 
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);
    }
}