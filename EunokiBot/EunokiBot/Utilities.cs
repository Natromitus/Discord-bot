using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using EunokiBot.Model;
using EunokiBot.ImageManagment;

namespace EunokiBot
{
    class Utilities
    {
        private static Dictionary<string, string> m_arAlerts;
        private static List<string> m_arCringe = null;
        private static List<string> m_arPuns = null;
        private static List<string> m_arFacts = null;

        public static List<string> Cringe
        {
            get
            {
                if (m_arCringe == null)
                {
                    string sJson = File.ReadAllText(Path.Combine(ImageManager.Singleton.FilePath, "cringe.json"));
                    Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(sJson);
                    m_arCringe = data.Values.ToList();

                }

                return m_arCringe;
            }
        }

        public static List<string> Puns
        {
            get
            {
                if (m_arPuns == null)
                {
                    string sJson = File.ReadAllText(Path.Combine(ImageManager.Singleton.FilePath, "puns.json"));
                    Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(sJson);
                    m_arPuns = data.Values.ToList();
                }

                return m_arPuns;
            }
        }


        public static List<string> Facts
        {
            get
            {
                if (m_arFacts == null)
                {
                    string sJson = File.ReadAllText(Path.Combine(ImageManager.Singleton.FilePath, "facts.json"));
                    Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(sJson);
                    m_arFacts = data.Values.ToList();
                }

                return m_arFacts;
            }
        }

        static Utilities()
        {
            string sJson = File.ReadAllText("SystemLang/alerts.json");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(sJson);
            m_arAlerts = data.ToObject<Dictionary<string, string>>();
        }   

        public static string GetAlert(string key, params object[] parameter)
        {
            if (m_arAlerts.ContainsKey(key))
                return string.Format(m_arAlerts[key], parameter);

            return null;
        }
    }
}
