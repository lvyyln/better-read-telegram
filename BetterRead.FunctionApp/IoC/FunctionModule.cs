using Autofac;
using Autofac.Extensions.DependencyInjection.AzureFunctions;
using AutoMapper;
using BetterRead.FunctionApp.Infrastructure;
using BetterRead.FunctionApp.Infrastructure.Abstractions;
using BetterRead.Shared;
using MediatR;

namespace BetterRead.FunctionApp.IoC
{
    public class FunctionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterThirdParty(builder);
            RegisterTriggers(builder);
            RegisterMediator(builder);
        }

        private static void RegisterThirdParty(ContainerBuilder builder)
        {
            builder
                .RegisterType<LoveRead>()
                .As<ILoveRead>();
            
            builder
                .RegisterType<CommandMatcher>()
                .As<ICommandMatcher>();
        }
        
        private static void RegisterTriggers(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(Startup).Assembly)
                .InNamespace("BetterRead.FunctionApp.Triggers")
                .AsSelf()
                .InstancePerTriggerRequest();
        }

        private static void RegisterMediator(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}