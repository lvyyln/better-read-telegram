using BetterRead.Commands.Abstractions;

namespace BetterRead.Commands.Error
{
    public class UnhandledCommand : ICommand
    {
        public long ChatId { get; set; }
        
        public string Name { get; set; }
    }
}