using System;
using System.IO;
using System.Reflection;
<<<<<<< HEAD
<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> parent of 6b00128... Patreon Init
=======
>>>>>>> parent of 6b00128... Patreon Init
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.WebSocket; 
using Discord.Commands;

using EunokiBot.Model;
using Patreon.NET;

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
        {
            const string sBaseUri = "www.patreon.com";
            // System.getProperty("patreon.rest.uri", "https://www.patreon.com"); ?? 

            // Get this when you set up your client
            string sClientId = "qAqYR_r5Ng5UZQPqLGBqTJfvcgilo4t93WybLH_5zwauSqcpWw6g8NWEW_7ZqIlZ";
            // Get this when you set up your client
            string sClientSecret = "UYqIgtcjLqBpiYsITi_Z0P6xPXcgzkArIimDJsUtqXhv_xNFlslRLH5oacXWahZM";
            // Provide this to set up your client
            string sRedirectUri = "https://www.instagram.com/eunokiofficial/";
            // Get this from the query parameter 'code'
            string sCode = "";

            PatreonOAuth oauthClient = new PatreonOAuth(sClientId, sClientSecret, sRedirectUri);
            PatreonOAuth.TokensResponse tokens = oauthClient.getTokens(code);



            //https://www.patreon.com/oauth2/authorize?response_type=code&client_id=qAqYR_r5Ng5UZQPqLGBqTJfvcgilo4t93WybLH_5zwauSqcpWw6g8NWEW_7ZqIlZ&redirect_uri=https://www.instagram.com/eunokiofficial/

            //var queryString = url.Substring(url.IndexOf('?')).Split('#')[0]
            //        System.Web.HttpUtility.ParseQueryString(queryString)

            /*
             * FROM JAVA API LIB
             
            URIBuilder builder = null;
            try
            {
                builder = new URIBuilder(PatreonAPI.BASE_URI + "/oauth2/authorize");
            }
            catch (URISyntaxException e)
            {
                LOG.error(e.getMessage());
            }
            builder.addParameter("response_type", "code");
            builder.addParameter("client_id", clientID);
            builder.addParameter("redirect_uri", redirectUri);
            return builder.toString();
            */


            //PatreonClient patreon = new PatreonClient(sAccessToken);
            //patreon.GET("GET www.patreon.com/oauth2/authorize" +
            //    "?response_type=code" +
            //    "&client_id=qAqYR_r5Ng5UZQPqLGBqTJfvcgilo4t93WybLH_5zwauSqcpWw6g8NWEW_7ZqIlZ" +
            //    "&redirect_uri=https://www.google.sk/");
            //Task<System.Net.Http.HttpResponseMessage> test = patreon.GET("https://www.patreon.com/api/oauth2/api/current_user/campaigns/data.id");
            //Task<Campaign> test2 = patreon.GET<Campaign>("https://www.patreon.com/api/oauth2/api/current_user/campaigns");
            //List <Pledge> pledges = patreon.GetCampaignPledges("CAMPAIGN_ID").Result;

            //Pridat Discord do Patreon.NET modelu

        }
        //=> new Program().MainAsync().GetAwaiter().GetResult();

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

            var patreon = new PatreonClient("ACCESS_TOKEN");
            var pledges = patreon.GetCampaignPledges("CAMPAIGN_ID").Result;

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
