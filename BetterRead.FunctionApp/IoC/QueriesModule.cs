using System.Reflection;
using Autofac;
using BetterRead.Commands.Abstractions;
using MediatR;

namespace BetterRead.FunctionApp.IoC
{
    public class QueriesModule : Autofac.Module
    {
        protected override Assembly ThisAssembly => typeof(CommandHandlerBase<>).Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();
        }
    }
}