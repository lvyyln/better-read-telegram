using BetterRead.Commands.Abstractions;

namespace BetterRead.Commands.Start
{
    public class StartCommand : ICommand
    {
        public long ChatId { get; set; }
        public string UserName { get; set; }
    }
}