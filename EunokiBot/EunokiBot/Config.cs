using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace EunokiBot
{
    class Config
    {
        private const string sConfigFolder = "Resources";
        private const string sConfigFile = "config.json";
        public static BotConfig Bot { get; private set; }

        static Config()
        {
            if (!Directory.Exists(sConfigFolder))
                Directory.CreateDirectory(sConfigFolder);

            string sFilePath = Path.Combine(sConfigFolder, sConfigFile);
            if (!File.Exists(sFilePath))
            {
                Bot = new BotConfig();
                string sJson = JsonConvert.SerializeObject(Bot, Formatting.Indented);
                File.WriteAllText(sFilePath, sJson);
            }
            else
            {
                string sJson = File.ReadAllText(sFilePath);
                Bot = JsonConvert.DeserializeObject<BotConfig>(sJson);
            }
        }
    }

    public struct BotConfig
    {
        public string token;
        public string cmdPrefix;
    }
}
