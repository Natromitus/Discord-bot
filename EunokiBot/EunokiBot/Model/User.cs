using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace EunokiBot.Model
{
    public class User : Root
    {
        #region Fields
        public const string TABLE_NAME = "Users";
        public const string PRIMARY_KEY = "UserID";
        private int m_nWarnings;
        private int m_nMessages;
        private int m_nLevel;
        private int m_nXP;
        private int m_nMoney;
        private int m_nQuests;
        private int m_nDailyCount;
        private string m_sDaily;
        private string m_sReroll;
        private string m_sJoinedDate;
        private int m_nQuestID1;
        private int m_nQuestID2;
        private int m_nQuestID3;
        private int m_nProgress1;
        private int m_nProgress2;
        private int m_nProgress3;
        #endregion

        #region Properties
        public long SQLUser_ID { get; private set; }
        public int Warnings
        {
            get { return m_nWarnings; }
            set
            {
                if (!SetField(ref m_nWarnings, value))
                    return;

                if (value >= 3)
                    _ = Program.Singleton.AlertsHandler.OnWarning(UserID, value);
            }
        }

        public int Messages
        {
            get { return m_nMessages; }
            set
            {
                if (!SetField(ref m_nMessages, value))
                    return;

                XP += SQL.Singleton.GetValue<int>("Levels", "XPPerMessage", "Level", Level);
            }
        }

        public int Level
        {
            get { return m_nLevel; }
            set
            {
                if (!SetField(ref m_nLevel, value))
                    return;

                _ = Program.Singleton.AlertsHandler.OnLevelUp(UserID, value);
            }
        }

        public int XP   
        {
            get { return m_nXP; }
            set
            {
                if(value < 0)
                {
                    if ((m_nXP - value) < 0)
                        value = 0;
                }

                if (!SetField(ref m_nXP, value))
                    return;

                if (m_nXP >= Data.Singleton.Levels[Level].XPGap)
                    ++Level;
            }
        }

        public int Money
        {
            get { return m_nMoney; }
            set
            {
                SetField(ref m_nMoney, value);
            }
        }

        public int Quests
        {
            get { return m_nQuests; }
            set
            {
                SetField(ref m_nQuests, value);
            }
        }

        public int DailyCount
        {
            get { return m_nDailyCount; }
            set
            {
                SetField(ref m_nDailyCount, value);
            }
        }

        public string Daily
        {
            get { return m_sDaily; }
            set
            {
                SetField(ref m_sDaily, value);
            }
        }

        public string Reroll
        {
            get { return m_sReroll; }
            set
            {
                SetField(ref m_sReroll, value);
            }
        }

        public string JoinedDate
        {
            get { return m_sJoinedDate; }
            set
            {
                SetField(ref m_sJoinedDate, value);
            }
        }

        public int QuestID1
        {
            get { return m_nQuestID1; }
            set
            {
                SetField(ref m_nQuestID1, value);
            }
        }

        public int Progress1
        {
            get { return m_nProgress1; }
            set
            {
                SetField(ref m_nProgress1, value);
            }
        }

        public int QuestID2
        {
            get { return m_nQuestID2; }
            set
            {
                SetField(ref m_nQuestID2, value);
            }
        }

        public int Progress2
        {
            get { return m_nProgress2; }
            set
            {
                SetField(ref m_nProgress2, value);
            }
        }

        public int QuestID3
        {
            get { return m_nQuestID3; }
            set
            {
                SetField(ref m_nQuestID3, value);
            }
        }

        public int Progress3
        {
            get { return m_nProgress3; }
            set
            {
                SetField(ref m_nProgress3, value);
            }
        }

        public IEnumerable<KeyValuePair<int, int>> CurrentQuests
        {
            get
            {
                yield return new KeyValuePair<int, int>(QuestID1, Progress1);
                yield return new KeyValuePair<int, int>(QuestID2, Progress2);
                yield return new KeyValuePair<int, int>(QuestID3, Progress3);
            }
        }

        public ulong UserID
        {
            get
            {
                return (ulong)SQLUser_ID;
            }
            set
            {
                SQLUser_ID = (long)value;
            }
        }
        #endregion

        public User(ulong id)
        {
            using (WriteSuspender wSus = new WriteSuspender())
            {
                UserID = id;
                Level = 1;
                Warnings = Messages = XP = Money = Quests = DailyCount = 0;
                JoinedDate = DateTime.Now.ToString("d. MM. yyyy");
                Reroll = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                AssignQuests();
            }
        }

        protected User()
        {
        }

        public static User Get(ulong ulUserID)
        {
            using(WriteSuspender wSus = new WriteSuspender())
            using (ReadSuspender rSus = new ReadSuspender())
            {
                return SQL.Singleton.Connection.Query<User>(
                    $"SELECT * FROM Users WHERE {PRIMARY_KEY} = {(long)ulUserID}").FirstOrDefault();
            }
        }

        public static void NewRecord(User user)
        {
           SQL.Singleton.Connection.Execute($"INSERT INTO {TABLE_NAME}" +
                $" ({PRIMARY_KEY}, Warnings, Messages, Level, XP, Money, Quests," +
                $" DailyCount, Daily, Reroll, JoinedDate," +
                $" QuestID1, Progress1, QuestID2, Progress2, QuestID3, Progress3)" +
                $" VALUES (@{PRIMARY_KEY}, @Warnings, @Messages, @Level, @XP, @Money, @Quests," +
                $" @DailyCount, @Daily, @Reroll, @JoinedDate," +
                $" @QuestID1, @Progress1, @QuestID2, @Progress2, @QuestID3, @Progress3)", user);
        }

        public bool AddProgressOnIndex(int index, int maxProgress)
        {
            switch (index)
            {
                case 0:
                    if (++Progress1 >= maxProgress)
                    {
                        QuestID1 = Progress1 = 0;
                        return true;
                    }
                    break;
                case 1:
                    if (++Progress2 >= maxProgress)
                    {
                        QuestID2 = Progress2 = 0;
                        return true;
                    }
                    break;
                case 2:
                    if (++Progress3 >= maxProgress)
                    {
                        QuestID3 = Progress3 = 0;
                        return true;
                    }
                    break;
            }

            return false;
        }

        public void RemoveProgressOnIndex(int index)
        {
            switch (index)
            {
                case 0:
                    if (--Progress1 < 0)
                    {
                        Progress1 = 0;
                        return;
                    }
                    break;
                case 1:
                    if (--Progress2 < 0)
                    {
                        Progress2 = 0;
                        return;
                    }
                    break;
                case 2:
                    if (--Progress3 < 0)
                    {
                        Progress3 = 0;
                        return;
                    }
                    break;
            }
        }

        public void AssignQuests()
        {
            Level lvl = Data.Singleton.Levels[Level - 1];

            for(int i = 0; i < 3; ++i)
            {
                Random rnd = new Random();
                float fRnd = (float)rnd.NextDouble();
                if (fRnd < lvl.ChanceEasyQ)
                {
                    SetUserQuest(i, Data.Singleton.EasyQuests[
                        rnd.Next(0, Data.Singleton.EasyQuests.Count)].QuestID);
                }
                else if(fRnd < lvl.ChanceEasyQ + lvl.ChanceMediumQ)
                {
                    SetUserQuest(i, Data.Singleton.MediumQuests[
                        rnd.Next(0, Data.Singleton.MediumQuests.Count)].QuestID);
                }
                else if (fRnd < lvl.ChanceEasyQ + lvl.ChanceMediumQ + lvl.ChanceHardQ)
                {
                    SetUserQuest(i, Data.Singleton.HardQuests[
                        rnd.Next(0, Data.Singleton.HardQuests.Count)].QuestID);
                }
                else if (fRnd < lvl.ChanceEasyQ + lvl.ChanceMediumQ + lvl.ChanceHardQ + lvl.ChanceLegendaryQ)
                {
                    SetUserQuest(i, Data.Singleton.LegendaryQuests[
                        rnd.Next(0, Data.Singleton.LegendaryQuests.Count)].QuestID);
                }
            }
        }

        private void SetUserQuest(int nIndex, int nID)
        {
            switch (nIndex)
            {
                case 0:
                    QuestID1 = nID;
                    Progress1 = 0;
                    break;
                case 1:
                    QuestID2 = nID;
                    Progress2 = 0;
                    break;
                case 2:
                    QuestID3 = nID;
                    Progress3 = 0;
                    break;
            }
        }

        protected override string OnGetTableName() => TABLE_NAME;
        protected override string OnGetPrimaryKeyName() => PRIMARY_KEY;
        protected override object OnGetPrimaryKeyValue() => SQLUser_ID;
    }
}
