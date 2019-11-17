using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.WebSocket; 

using Patreon.NET;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace EunokiBot
{
    class Program
    {
        #region Fields
        private static Program m_singleton;
        private DiscordSocketClient _client;
        private CommandHandler _cmdHandler;
        private Alerts _alertHandler;

        
        #endregion
            
        #region Properties
        public static Program Singleton => m_singleton;
        public DiscordSocketClient Client => _client;
        public Alerts AlertsHandler => _alertHandler;
        #endregion

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            m_singleton = this;

            if (String.IsNullOrEmpty(Config.Bot.token))
                return;
            
            _client = new DiscordSocketClient(new DiscordSocketConfig
            { LogLevel = LogSeverity.Verbose });

            _client.Log += LogAsync;
            _client.ReactionAdded += ReactManager.Singleton.ReactionAddedAsync;
            _client.ReactionRemoved += ReactManager.Singleton.ReactionRemovedAsync;

            await _client.LoginAsync(TokenType.Bot, Config.Bot.token);
            await _client.StartAsync();

            _cmdHandler = new CommandHandler();
            await _cmdHandler.InitializeAsync(_client);

            _alertHandler = new Alerts();
            _client.UserJoined += _alertHandler.OnUserJoin;

            await Task.Delay(-1);
        }

        private async Task LogAsync(LogMessage sMsg)
        {
            Console.WriteLine(sMsg.Message);
            if(sMsg.Exception != null)
                Console.WriteLine(sMsg.Exception);
        }
    }
}
