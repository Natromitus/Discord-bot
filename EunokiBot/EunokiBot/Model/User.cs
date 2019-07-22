namespace EunokiBot.Model
{
    public class User
    {
        public long SQLUser_ID { get; private set; }
        public int Warnings { get; private set; }
        public int Messages { get; private set; }

        public int Level { get; private set; }
        public int XP { get; private set; }
        /*
        {
            get { return m_nXP; }
            set
            {
                m_nXP = value;
                if (m_nXP >= Data.Data.GetLevelGapByIndex(Level + 1))
                    ++Level;
            }
        }*/
        public int Money { get; private set; }
        public int Quests { get; private set; }
        public int Wins { get; private set; }
        public int Lost { get; private set; }

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

        private int m_nXP;

        public User(ulong id)
        {
            UserID = id;
            Warnings = Messages = XP = Money = Quests = Wins = Lost = 0;
            Level = 1;
        }

        public User()
        {

        }
    }
}
