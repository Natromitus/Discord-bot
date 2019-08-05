using System.Linq;
using Dapper;

namespace EunokiBot.Model
{
    public class User : Root
    {
        #region Fields
        private const string m_sTableName = "Users";
        private const string m_sPrimaryKey = "UserID";
        private int m_nWarnings;
        private int m_nMessages;
        private int m_nLevel;
        private int m_nXP;
        private int m_nMoney;
        private int m_nQuests;
        private int m_nWins;
        private int m_nLost;
        #endregion

        #region Properties
        public long SQLUser_ID { get; private set; }
        public int Warnings
        {
            get { return m_nWarnings; }
            set
            {
                if (!SetField<int>(ref m_nMessages, value))
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
                if (!SetField<int>(ref m_nMessages, value))
                    return;

                XP += SQL.Singleton.GetValue<int>("Levels", "XPPerMessage", "Level", Level);
            }
        }

        public int Level
        {
            get { return m_nLevel; }
            set
            {
                if (!SetField<int>(ref m_nLevel, value))
                    return;

                _ = Program.Singleton.AlertsHandler.OnLevelUp(UserID, value);
            }
        }

        public int XP
        {
            get { return m_nXP; }
            set
            {
                if (!SetField<int>(ref m_nXP, value))
                    return;

                if (m_nXP >= SQL.Singleton.GetLevelGapByIndex(Level + 1))
                    ++Level;
            }
        }

        public int Money
        {
            get { return m_nMoney; }
            set
            {
                SetField<int>(ref m_nMoney, value);
            }
        }

        public int Quests
        {
            get { return m_nQuests; }
            set
            {
                SetField<int>(ref m_nQuests, value);
            }
        }

        public int Wins
        {
            get { return m_nWins; }
            set
            {
                SetField<int>(ref m_nWins, value);
            }
        }

        public int Lost
        {
            get { return m_nLost; }
            set
            {
                SetField<int>(ref m_nLost, value);
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
                    $"SELECT * FROM Users WHERE {m_sPrimaryKey} = {(long)ulUserID}").FirstOrDefault();
            }
        }

        public static void NewRecord(User user)
        {
            SQL.Singleton.Connection.Execute($"INSERT INTO {m_sTableName}" +
                $"({m_sPrimaryKey}, Warnings, Messages, Level, XP, Money, Quests, Wins, Lost)" +
                $" VALUES (@{m_sPrimaryKey}, @Warnings, @Messages, @Level, @XP, @Money, @Quests, @Wins, @Lost)", user);
        }

        protected override string OnGetTableName() => m_sTableName;
        protected override string OnGetPrimaryKeyName() => m_sPrimaryKey;
        protected override object OnGetPrimaryKeyValue() => SQLUser_ID;
    }
}
