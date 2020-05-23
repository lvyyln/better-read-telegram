using BetterRead.Commands.Abstractions;

namespace BetterRead.Commands.Documents
{
    public class DocumentCommand : ICommand
    {
        public long ChatId { get; set; }
        public int BookId { get; set; }
    }
}