using AutoMapper;
using BetterRead.Commands.Error;
using BetterRead.Commands.Validation;
using Telegram.Bot.Types;

namespace BetterRead.FunctionApp.Mappings
{
    public class ValidationProfile : Profile
    {
        public ValidationProfile()
        {
            CreateMap<Update, ValidationLinkCommand>()
                .ForMember(c => c.ChatId, opt => opt.MapFrom(upd => upd.Message.Chat.Id))
                .ForMember(c => c.Name, opt => opt.MapFrom(upd => upd.Message.From.Username));
        }
    }
}