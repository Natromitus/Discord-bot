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
        private int m_nWins;
        private int m_nLost;
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
                if (!SetField(ref m_nXP, value))
                    return;

                if (m_nXP >= Data.Singleton.Levels[Level + 1].XPGap)
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

        public int Wins
        {
            get { return m_nWins; }
            set
            {
                SetField(ref m_nWins, value);
            }
        }

        public int Lost
        {
            get { return m_nLost; }
            set
            {
                SetField(ref m_nLost, value);
            }
        }

        private int QuestID1
        {
            get { return m_nQuestID1; }
            set
            {
                SetField(ref m_nQuestID1, value);
            }
        }

        private int Progress1
        {
            get { return m_nProgress1; }
            set
            {
                SetField(ref m_nProgress1, value);
            }
        }

        private int QuestID2
        {
            get { return m_nQuestID2; }
            set
            {
                SetField(ref m_nQuestID2, value);
            }
        }

        private int Progress2
        {
            get { return m_nProgress2; }
            set
            {
                SetField(ref m_nProgress2, value);
            }
        }

        private int QuestID3
        {
            get { return m_nQuestID3; }
            set
            {
                SetField(ref m_nQuestID3, value);
            }
        }

        private int Progress3
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
            UserID = id;
            Warnings = Messages = XP = Money = Quests = Wins = Lost = 0;
            Level = 1;
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
                $"({PRIMARY_KEY}, Warnings, Messages, Level, XP, Money, Quests, Wins, Lost)" +
                $" VALUES (@{PRIMARY_KEY}, @Warnings, @Messages, @Level, @XP, @Money, @Quests, @Wins, @Lost)", user);
        }

        protected override string OnGetTableName() => TABLE_NAME;
        protected override string OnGetPrimaryKeyName() => PRIMARY_KEY;
        protected override object OnGetPrimaryKeyValue() => SQLUser_ID;
    }
}
