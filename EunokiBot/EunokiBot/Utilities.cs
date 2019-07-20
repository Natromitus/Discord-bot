using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace EunokiBot
{
    class Utilities
    {
        private static Dictionary<string, string> m_arAlerts;

        static Utilities()
        {
            string sJson = File.ReadAllText("SystemLang/alerts.json");
            var data = JsonConvert.DeserializeObject<dynamic>(sJson);
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
