using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Dapper;
using Microsoft.Data.Sqlite;
using SQLite.Net;
using SQLite.Net.Async;

namespace EunokiBot.Data
{
    public static class Data
    {
        private static async Task<String> LoadConnectionString()
        {
            string sLocation = Assembly.GetEntryAssembly().Location.Replace(@"EunokiBot.dll", @"Database\DemoDb.db");
            string sConnectionString = "Data Source=" + sLocation + ";Version=3";
            return sConnectionString;
        }
            

       /* public static List<UserModel> GetUser(ulong UserID)
        {
            /*
            using (IDbConnection cnn = new SqliteConnection(await LoadConnectionString()))
            {
                var output = cnn.Query<UserModel>("select * from Users", new DynamicParameters());
                return output.ToList();
            }

            
            using(var dbContext = new SqliteDbContext())
            {
                if (!dbContext.Users.Any(obj => obj.UserID == UserID))
                    return null;

                return dbContext.Users.Where(obj => obj.UserID == UserID).FirstOrDefault();
            }
        }*/

        public static async Task SaveUser(UserModel user)
        {
            var conn = new SQLiteAsyncConnection("file1.db");













            cnn.Execute("insert into Users (USER_ID, WARNINGS, LEVEL, XP, MONEY, QUESTS, WINS, LOST)" +
                "values (@UserID, @Warnings, @Level, @XP, @Money, @Quests, @Wins, @Lost)", user);

            cnn.Execute("insert into User (@UserID, @Warnings, @Level, @XP, @Money, @Quests, @Wins, @Lost)" +
                "values (USER_ID, WARNINGS, LEVEL, XP, MONEY, QUESTS, WINS, LOST)", user);

            cnn.Execute("insert into Users (USER_ID, WARNINGS, LEVEL, XP, MONEY, QUESTS, WINS, LOST)" +
                "values (@UserID, @Warnings, @Level, @XP, @Money, @Quests, @Wins, @Lost)", user);
            












            /*
             * 

            cnn.Execute("insert into Users (@UserID, @Warnings, @Level, @XP, @Money, @Quests, @Wins, @Lost)" +
                    "values (USER_ID, WARNINGS, LEVEL, XP, MONEY, QUESTS, WINS, LOST)", user);

                                cnn.Execute("insert into Users (USER_ID, WARNINGS, LEVEL, XP, MONEY, QUESTS, WINS, LOST)" +
                    "values (@UserID, @Warnings, @Level, @XP, @Money, @Quests, @Wins, @Lost)", user);
             * 
            using (var dbContext = new SqliteDbContext())
            {
                if (dbContext.Users.Any(obj => obj.UserID == UserID))
                {
                    dbContext.Users.Add(new UserModel
                    {
                        UserID = UserID,
                        Warnings = 0,
                        Level = 1,
                        XP = 0
                    });
                }
                else
                {
                    UserModel current = dbContext.Users.Where(obj => obj.UserID == UserID).FirstOrDefault();
                    dbContext.Users.Update(current);
                }

                await dbContext.SaveChangesAsync();
            }*/
        }
    }
}
