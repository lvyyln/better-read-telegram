using System;
using AutoMapper;
using BetterRead.Commands.Abstractions;
using BetterRead.Commands.Books;
using BetterRead.Commands.Documents;
using BetterRead.Commands.Error;
using BetterRead.Commands.Start;
using BetterRead.Commands.Validation;
using BetterRead.FunctionApp.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Builder.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BetterRead.FunctionApp.Infrastructure
{
    public class CommandMatcher : ICommandMatcher
    {
        private readonly IMapper _mapper;

        public CommandMatcher(IMapper mapper) =>
            _mapper = mapper;

        public ICommand MatchCommandAsync(Update update) =>
            update.Type switch
            {
                UpdateType.MessageUpdate => MatchMessageUpdate(update),
                UpdateType.CallbackQueryUpdate => MatchCallbackQueryUpdate(update),
                _ => MatchUnhandledUpdate(update)
            };

        private ICommand MatchMessageUpdate(Update update) =>
            update switch
            {
                var upd when upd.Message.Text.Contains("/start") =>
                _mapper.Map<StartCommand>(upd),
                var upd when upd.Message.Text.Contains("http://loveread.ec/view_global.php?id=") =>
                _mapper.Map<BookInfoCommand>(upd),
                _ => MatchUnhandledUpdate(update)
            };

        private ICommand MatchCallbackQueryUpdate(Update update) =>
            update switch
            {
                var upd when
                upd.CallbackQuery.Message.Text.Contains("Author") &&
                upd.CallbackQuery.Message.Text.Contains("Book") =>
                _mapper.Map<DocumentCommand>(upd),
                _ => MatchUnhandledUpdate(update)
            };

        private ICommand MatchUnhandledUpdate(Update update)
            => update switch
            {
                _ => _mapper.Map<UnhandledCommand>(update),
            };
    }
}