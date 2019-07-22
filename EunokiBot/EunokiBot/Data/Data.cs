using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

using EunokiBot.Model;

using Dapper;

namespace EunokiBot.Data
{
    public static class Data
    {
        private static string m_sConnectionString = "Data Source="
            + Assembly.GetEntryAssembly().Location.Replace(@"EunokiBot.dll", @"Database\DemoDb.db");

        #region Get
        public static User GetUser(ulong ulUserID)
        {
            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                List<User> user = cnn.Query<User>($"SELECT * from Users WHERE USER_ID = {(long)ulUserID}").ToList();
                if (user.Count == 0)
                    return null;

                return user.First();
            }
        }

        public static Inventory GetInventory(ulong ulUserID)
        {
            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                List<Inventory> inventory = cnn.Query<Inventory>($"SELECT * from Inventory WHERE USER_ID = {(long)ulUserID}").ToList();
                if (inventory.Count == 0)
                    return null;

                return inventory.First();
            }
        }

        public static Item GetItemByID(int nID)
        {
            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                List<Item> item = cnn.Query<Item>($"SELECT * from Items WHERE ITEM_ID = {nID}").ToList();
                if (item.Count == 0)
                    return null;

                return item.First();
            }
        }

        public static Item GetItemAtUserIndex(ulong ulUserID, int nIndex)
        {
            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                if (nIndex < 0 || nIndex > 2)
                    return null;

                Inventory inventory = GetInventory(ulUserID);
                if (inventory == null)
                    return null;

                Item item = null;
                switch (nIndex)
                {
                    case 0:
                        item = GetItemByID(inventory.ItemID0);
                        break;
                    case 1:
                        item = GetItemByID(inventory.ItemID1);
                        break;
                    case 2:
                        item = GetItemByID(inventory.ItemID2);
                        break;
                }
                return item;
            }
        }

        public static int GetLevelGapByIndex(int nIndex)
        {
            // TODO IF THERE IS THAT INDEX

            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                return cnn.Query<int>($"SELECT GAP FROM Levels WHERE LEVEL = {nIndex}").FirstOrDefault();
            }
        }
        #endregion

        #region Set
        #region Create
        public static void CreateUser(User user)
        {
            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                cnn.Execute("insert into Users (USER_ID, WARNINGS, MESSAGES, LEVEL, XP, MONEY, QUESTS, WINS, LOST)" +
                    " values (@UserID, @Warnings, @Messages, @Level, @XP, @Money, @Quests, @Wins, @Lost)", user);
            }
        }

        public static void CreateInventory(Inventory inventory)
        {
            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                cnn.Execute("insert into Inventory (USER_ID, ITEM_ID 0, AMOUNT 0, ITEM_ID 1, AMOUNT 1, ITEM_ID 2, AMOUNT 2)" +
                    " values (@UserID ,@ItemID0, @Amount0, @ItemID1, @Amount1, @UserID2, @Amount2)", inventory);
            }
        }
        #endregion
        public static void IncrementUserMessage(ulong ulUserID)
        {
            User user = GetUser(ulUserID);
            if (user == null)
                return;

            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                cnn.Execute($"UPDATE Users SET MESSAGES = {user.Messages + 1} WHERE USER_ID = {(long)ulUserID}");

                // TODO: Don't let it go after max gap.
                cnn.Execute($"UPDATE Users SET XP = {user.XP + 2} WHERE USER_ID = {(long)ulUserID}");

                if ((user.XP + 2) >= GetLevelGapByIndex(user.Level + 1))
                    LevelUpUser(ulUserID);
            }
        }

        public static void LevelUpUser(ulong ulUserID)
        {
            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                // TODO: CHECK IF NOT MAX LVL
                int nLevel = cnn.Query<int>($"SELECT LEVEL FROM USERS WHERE USER_ID = {ulUserID}").FirstOrDefault();
                cnn.Execute($"UPDATE Users SET LEVEL = {nLevel + 1} WHERE USER_ID = {(long)ulUserID}");
            }
        }

        public static void SetItemAtIndex(ulong ulUserID, int nIndex, int nItemID, int nAmount)
        {
            if (nIndex < 0 || nIndex > 2)
                return;

            Item item = GetItemByID(nItemID);
            if (item == null)
                return;

            if (item.MaxStack < nAmount)
                return;

            using (IDbConnection cnn = new SQLiteConnection(m_sConnectionString))
            {
                cnn.Execute($"UPDATE Inventory SET ITEM_ID {nIndex} = {nItemID}, AMOUNT {nIndex} = {nAmount} WHERE USER_ID = {(long)ulUserID}");
            }
        }
        #endregion
    }
}
