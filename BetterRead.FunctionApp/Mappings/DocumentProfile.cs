using AutoMapper;
using BetterRead.Commands.Documents;
using Telegram.Bot.Types;

namespace BetterRead.FunctionApp.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<Update, DocumentCommand>()
                .ForMember(c => c.ChatId, opt => opt.MapFrom(u => u.CallbackQuery.Message.Chat.Id))
                .ForMember(c => c.BookId, opt => opt.MapFrom(u => int.Parse(u.CallbackQuery.Data)));
        }
    }
}