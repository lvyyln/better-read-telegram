using System.Threading.Tasks;
using BetterRead.Commands.Abstractions;
using BetterRead.MediatR.Core.HandlerResults.Abstractions;
using Telegram.Bot.Types;

namespace BetterRead.FunctionApp.Infrastructure.Abstractions
{
    public interface ICommandMatcher
    {
        public ICommand MatchCommandAsync(Update update);
    }
}