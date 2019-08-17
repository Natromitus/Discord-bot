using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

using EunokiBot.Model;

using Dapper;

namespace EunokiBot
{
    public class SQL
    {
        private static readonly SQL m_singleton = new SQL();

        private IDbConnection m_cnn;

        // Properties
        public static SQL Singleton => m_singleton;

        public IDbConnection Connection
        {
            get
            {
                if (m_cnn != null)
                    return m_cnn;

                string m_sConnectionString = "Data Source=" +
                    Assembly.GetEntryAssembly().Location.Replace(@"EunokiBot.dll", @"Database\Main.db");
                m_cnn = new SQLiteConnection(m_sConnectionString);
                return m_cnn;
            }
        }

        #region Get
        public T GetValue<T>(string sTable, string sProperty, string sKeyName, object keyVal)
        {
            return Connection.Query<T>($"SELECT {sProperty} FROM {sTable} WHERE {sKeyName} = {keyVal}").FirstOrDefault();
        }

        public int GetCount(string sTable)
        {
            return Connection.Query<int>($"SELECT COUNT(*) FROM {sTable}").FirstOrDefault();
        }
        #endregion

        #region Set
        public void UpdateValue(string sTable, string sVar, object value, string sKeyName, object keyVal)
        {
            Connection.Execute($"UPDATE {sTable} SET {sVar} = {value} WHERE {sKeyName} = {keyVal}");
        }
        #endregion
    }
}
