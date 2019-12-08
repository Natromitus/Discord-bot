using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

using Discord.WebSocket;
using Discord.Commands;

using EunokiBot.Model;
using EunokiBot.Quests;

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
            User user = User.Get(context.User.Id);
            if (user != null)
            {
                user.Messages++;
                ActionParam action = null;

                if (sMessage.Attachments.Count > 0)
                {
                    string sFileName = sMessage.Attachments.ToArray()[0].Filename;
                    bool bIsImage = Data.Singleton.ImageEndings.Any(obj => sFileName.EndsWith(obj));
                    if (bIsImage)
                        action = new ActionParam("Pictures", context.Channel.Id);

                }
                else
                    action = new ActionParam("Messages", context.Channel.Id);

                ActionManager.Singleton.OnAction(user, action);
            }

            int nArsPos = 0;

            if (sMessage.HasStringPrefix(Config.Bot.cmdPrefix, ref nArsPos) ||
                sMessage.HasMentionPrefix(_client.CurrentUser, ref nArsPos))
            {
                if (sMessage.Channel.Id == Convert.ToUInt64(Config.Bot.channelBotCommands) ||
                    sMessage.Channel.GetType() == typeof(SocketDMChannel))
                {
                    IResult result = await _service.ExecuteAsync(context, nArsPos, services: null);
                    if (!result.IsSuccess && result.Error == CommandError.UnknownCommand)
                        Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
