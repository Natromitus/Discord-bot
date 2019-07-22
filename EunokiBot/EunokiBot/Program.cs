using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot
{
    class Program
    {
        DiscordSocketClient _client;
        CommandHandler _cmdHandler;

        public DiscordSocketClient Client { get => _client; }

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            if (String.IsNullOrEmpty(Config.Bot.token))
                return;

            _client = new DiscordSocketClient(new DiscordSocketConfig
            { LogLevel = LogSeverity.Verbose });

            _client.Log += Log;
            await Client.LoginAsync(TokenType.Bot, Config.Bot.token);
            await Client.StartAsync();

            _cmdHandler = new CommandHandler();
            await _cmdHandler.InitializeAsync(Client);

            //_joinHandler = new JoinHandler();
            //Client.UserJoined += _joinHandler.OnUserJoin;
            _client.UserJoined += OnUserJoin;

            await Task.Delay(-1);
        }

        public async Task OnUserJoin(SocketGuildUser user)
        {
            User userModel = Data.Data.GetUser(user.Id);
            if (userModel == null)
                Data.Data.CreateUser(new User(user.Id));

            var channel = _client.GetChannel(573131665660968972) as SocketTextChannel;
            if(channel == null)
                return;

            await channel.SendMessageAsync(Utilities.GetAlert("WELCOME_&MENTION_&GUILDNAME", user.Mention, channel.Guild.Name));
            SocketGuild guild = _client.GetGuild(573131665660968970) as SocketGuild;
            if (guild == null)
                return;

            IRole role = channel.Guild.Roles.FirstOrDefault(obj => obj.Name.ToString() == "Test Role");

            IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("JOINDM_&NAME", user.Mention));

            await user.AddRoleAsync(role);
        }

        private async Task Log(LogMessage sMsg)
        {
            Console.WriteLine(sMsg.Message);
        }
    }
}
