using AutoMapper;
using BetterRead.Commands.Books;
using Telegram.Bot.Types;

namespace BetterRead.FunctionApp.Mappings
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Update, BookInfoCommand>()
                .ForMember(c => c.ChatId, opt => opt.MapFrom(u => u.Message.Chat.Id))
                .ForMember(c => c.Url, opt => opt.MapFrom(u => u.Message.Text));
        }
    }
}