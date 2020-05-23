using System.Threading.Tasks;
using BetterRead.Shared;
using BetterRead.Shared.Domain.Books;

namespace BetterRead.OLD
{
    public interface IBookService
    {
        public Task<BookInfo> GetBookInfo(string bookUrl);
    } 
    public class BookService : IBookService
    {
        public async Task<BookInfo> GetBookInfo(string bookUrl)
        {
            var loveReadService = new LoveRead();
            return await loveReadService.GetBookInfoAsync(bookUrl);
        } 
    }
}