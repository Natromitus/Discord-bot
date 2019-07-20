using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

using Dapper;

namespace EunokiBot.Data
{
    public static class Data
    {
        private static string m_sConnectionString = "Data Source="
            + Assembly.GetEntryAssembly().Location.Replace(@"EunokiBot.dll", @"Database\DemoDb.db");

        public static UserModel GetUser(ulong ulUserID)
        {
            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                List<UserModel> user = cnn.Query<UserModel>($"SELECT * from Users WHERE USER_ID = {ulUserID}").ToList();
                if (user.Count == 0)
                    return null;

                return user.First();
            }
        }

        public static void SaveUser(UserModel user)
        {
            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                cnn.Execute("insert into Users (USER_ID, WARNINGS, MESSAGES, LEVEL, XP, MONEY, QUESTS, WINS, LOST)" +
                    " values (@UserID, @Warnings, @Messages, @Level, @XP, @Money, @Quests, @Wins, @Lost)", user);
            }
        }
    }
}
