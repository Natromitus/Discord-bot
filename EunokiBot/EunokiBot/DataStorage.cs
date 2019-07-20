using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace EunokiBot
{
    class DataStorage
    {
        private static Dictionary<string, string> m_arPairs = new Dictionary<string, string>();

        public static int PairsCount => m_arPairs.Count;
        static DataStorage()
        {
            // Load data
            string sFile = "DataStorage.json";
            if (!ValidateStorageFile(sFile))
                return;

            string sJson = sJson = File.ReadAllText(sFile);
            m_arPairs = JsonConvert.DeserializeObject<Dictionary<string, string>> (sJson);
        }

        public static void AddPairToStorage(string sKey, string sValue)
        {
            m_arPairs.Add(sKey, sValue);
            SaveData();
        }

        internal static void SaveData()
        {
            // Save Data
            string sJson = JsonConvert.SerializeObject(m_arPairs, Formatting.Indented);
            File.WriteAllText("DataStorage.json", sJson);
        }

        private static bool ValidateStorageFile(string sFile)
        {
            if (File.Exists(sFile))
                return true;

            File.WriteAllText(sFile, "");
            SaveData();
            return false;
        }
    }
}
