namespace EunokiBot
{
    public class UserModel
    {
        public ulong UserID { get; set; }
        public int Warnings { get; set; }
        public int Messages { get; set; }

        public int Level { get; set; }
        public int XP { get; set; }
        public int Money { get; set; }
        public int Quests { get; set; }
        public int Wins { get; set; }
        public int Lost { get; set; }

        public UserModel(ulong id)
        {
            UserID = id;
            Warnings = Messages = XP = Money = Quests = Wins = Lost = 0;
            Level = 1;
        }

        public UserModel()
        {

        }
    }
}
