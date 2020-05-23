using BetterRead.Commands.Abstractions;

namespace BetterRead.Commands.Books
{
    public class BookInfoCommand : ICommand
    {
        public long ChatId { get; set; }
        public string Url { get; set; }
    }
}