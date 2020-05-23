using BetterRead.Commands.Abstractions;

namespace BetterRead.Commands.Validation
{
    public class ValidationLinkCommand : ICommand
    {
        public long ChatId { get; set; }
        
        public string Name { get; set; }
    }
}