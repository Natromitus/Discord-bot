using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Patreon.NET;

using Newtonsoft.Json.Linq;
using System.IO;
using EunokiBot.ImageManagment;
using System.Reflection;

namespace EunokiBot
{
    public class PatreonManager
    {
        private const string ACCESS_TOKEN = "_vjyZPY3XakWb9hC02YZU5dd-ZJFzjuayZbd6DkS1QY";
        private const string PATREON = "https://www.patreon.com";
        private const string OAUTH = "/api/oauth2";
        private const string CURRENT_USER = "/api/current_user";

        private static PatreonManager m_singleton = new PatreonManager();

        private PatreonClient m_patreon = null;
        private string m_sCampaignID = null;
        private string m_sJson = null;

        public static PatreonManager Singleton => m_singleton;

        public PatreonClient Client
        {
            get
            {
                if (m_patreon == null)
                    m_patreon = new PatreonClient(ACCESS_TOKEN);

                return m_patreon;
            }
        }

        public async Task<string> GetCampaignID()
        {
            if (m_sCampaignID == null)
            {
                HttpResponseMessage responseUserCampaign = await Client.GET(PATREON + OAUTH + CURRENT_USER + "/campaigns");
                responseUserCampaign.EnsureSuccessStatusCode();
                string sUserCampaigns = await responseUserCampaign.Content.ReadAsStringAsync();
                m_sCampaignID = JObject.Parse(sUserCampaigns)["included"][0]["relationships"]["campaign"]["data"]["id"].ToString();
            }

            return m_sCampaignID;
        }

        public string Json
        {
            get
            {
                if (string.IsNullOrEmpty(m_sJson))
                {
                    string sPath = Assembly.GetEntryAssembly().Location.Replace(@"EunokiBot.dll", "");
                    m_sJson = File.ReadAllText(Path.Combine(sPath, "patreon.json"));
                }

                return m_sJson;
            }
        }

        public async Task<List<Patreon.NET.User>> GetPatronsAsync()
        {
            return null;
        }

        public async Task<List<ulong>> GetDiscordIDsAsync()
        {
            return null;
        }

        public List<ulong> CheckDiscordPatrons(List<ulong> patronDiscordIDs)
        {
            return null;
        }

        public void RemovePatrons(List<ulong> patronDiscordIDs)
        {
            // foreach loop that calls RemoveXPBoost
        }

        public void RewardPatrons(List<Patreon.NET.User> patrons)
        {

        }

        public void RewardCasual(ulong discordID) => GiveXPBoost(discordID);

        public void GiveXPBoost(ulong discordID)
        {
            // TODO XP BOOST
        }

        public void RemoveXPBoost(ulong discordID)
        {
            // TODO XP BOOST
        }

        public void RewardSerious(ulong discordID)
        {
            Model.User user = Model.User.Get(discordID);
            if (user == null)
                return;

            Model.Inventory inventory = Model.Inventory.Get(discordID);
            if (inventory == null)
                return;

            GiveXPBoost(discordID);
            inventory.AddItem(13, 5);
        }

        public void RewardHonesty(ulong discordID)
        {
            Model.User user = Model.User.Get(discordID);
            if (user == null)
                return;

            Model.Inventory inventory = Model.Inventory.Get(discordID);
            if (inventory == null)
                return;

            GiveXPBoost(discordID);
            inventory.AddItem(13, 15);
        }
    }
}
