using AutoMapper;
using BetterRead.Commands.Start;
using Telegram.Bot.Types;

namespace BetterRead.FunctionApp.Mappings
{
    public class StartProfile : Profile
    {
        public StartProfile()
        {
            CreateMap<Update, StartCommand>()
                .ForMember(c => c.ChatId, opt => opt.MapFrom(upd => upd.Message.Chat.Id))
                .ForMember(c => c.UserName, opt => opt.MapFrom(upd => upd.Message.From.Username));
        }
    }
}