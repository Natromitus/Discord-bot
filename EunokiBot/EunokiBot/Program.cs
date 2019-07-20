using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace EunokiBot
{
    class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            if (String.IsNullOrEmpty(Config.Bot.token))
                return;

            _client = new DiscordSocketClient(new DiscordSocketConfig
            { LogLevel = LogSeverity.Verbose });

            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, Config.Bot.token);
            await _client.StartAsync();

            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private async Task Log(LogMessage sMsg)
        {
            Console.WriteLine(sMsg.Message);
        }
    }
}
