using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Discord.WebSocket;
using Discord.Commands;

namespace EunokiBot
{
    class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), services: null);
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage sMsg)
        {
            SocketUserMessage sMessage = sMsg as SocketUserMessage;
            if (sMessage == null)
                return;

            SocketCommandContext context = new SocketCommandContext(_client, sMessage);
            Data.Data.IncrementUserMessage(context.User.Id);

            int nArsPos = 0;

            if (sMessage.HasStringPrefix(Config.Bot.cmdPrefix, ref nArsPos) ||
                sMessage.HasMentionPrefix(_client.CurrentUser, ref nArsPos))
            {
                IResult result = await _service.ExecuteAsync(context, nArsPos, services: null);
                if(!result.IsSuccess && result.Error == CommandError.UnknownCommand)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
