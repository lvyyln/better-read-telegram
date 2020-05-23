using AutoMapper;
using BetterRead.Commands.Error;
using Telegram.Bot.Types;

namespace BetterRead.FunctionApp.Mappings
{
    public class UnhandledProfile : Profile
    {
        public UnhandledProfile()
        {
            CreateMap<Update, UnhandledCommand>()
                .ForMember(c => c.ChatId, opt => opt.MapFrom(upd => upd.Message.Chat.Id))
                .ForMember(c => c.Name, opt => opt.MapFrom(upd => upd.Message.From.Username));
        }
    }
}