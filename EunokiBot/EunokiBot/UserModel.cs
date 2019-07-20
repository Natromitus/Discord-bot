using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EunokiBot
{
    public class UserModel
    {
        [Key]
        public ulong UserID { get; set; }
        public int Warnings { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public int Money { get; set; }
        public int Quests { get; set; }
        public int Wins { get; set; }
        public int Lost { get; set; }

        private UserModel() { }

        public static async Task<UserModel> CreateAsync(ulong id)
        {
            UserModel obj = new UserModel();
            await obj.InitializeAsync(id);
            return obj;
        }

        private async Task InitializeAsync(ulong id)
        {
            UserID = id;
            Warnings = XP = Money = Quests = Wins = Lost = 0;
            Level = 1;
        }

        // Items?

        // Quests

        // Accomplishments
        // public int FinishedQuests { get; set; }
        // Event trophies

        // public int Effect
    }
}
