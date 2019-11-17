using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Patreon.NET;

using Newtonsoft.Json.Linq;

namespace EunokiBot
{
    public class PatreonManager
    {
        private const string ACCESS_TOKEN = "_vjyZPY3XakWb9hC02YZU5dd-ZJFzjuayZbd6DkS1QY";
        private const string PATREON = "https://www.patreon.com";
        private const string OAUTH = "/api/oauth2";
        private const string CURRENT_USER = "/api/current_user/";

        private static PatreonManager m_singleton = new PatreonManager();

        private PatreonClient m_patreon = null;
        private string m_sCampaignID = null;


        public static PatreonManager Singleton => m_singleton;

        public PatreonClient Client
        {
            get
            {
                if(m_patreon == null)
                    m_patreon = new PatreonClient(ACCESS_TOKEN);

                return m_patreon;
            }
        }

        public async Task<string> GetCampaignID()
        {
            if(m_sCampaignID == null)
            {
                HttpResponseMessage responseUserCampaign = await Client.GET(PATREON + OAUTH + CURRENT_USER + "/campaigns");
                responseUserCampaign.EnsureSuccessStatusCode();
                string sUserCampaigns = await responseUserCampaign.Content.ReadAsStringAsync();
                string m_sCampaignID = JObject.Parse(sUserCampaigns)["included"][0]["relationships"]["campaign"]["data"]["id"].ToString();
            }

            return m_sCampaignID;
        }

        public async Task<List<ulong>> GetDiscordIDs()
        {
            return null;
        }
    }
}
