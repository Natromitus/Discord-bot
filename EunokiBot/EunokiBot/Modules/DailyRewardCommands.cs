using Discord.Commands;
using EunokiBot.ImageManagment;
using EunokiBot.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EunokiBot.Modules
{
    [Group("daily"), Summary("Daily Reward commands.")]
    public class DailyRewardCommands : ModuleBase<SocketCommandContext>
    {
        [Command(""), Alias("info"), Summary("List of all daily rewards.")]
        public async Task DailyRewardsInfoAsync()
        {
            User user = User.Get(Context.User.Id);
            if (user == null)
                return;

            string sImageFileName = ImageManager.Singleton.DailyRewards(user);
            if (sImageFileName == string.Empty)
                return;

            await Context.Channel.SendFileAsync(Path.Combine(ImageManager.Singleton.FilePath, sImageFileName));
            File.Delete(Path.Combine(ImageManager.Singleton.FilePath, sImageFileName));

            // Show ImageManager generated picture of Daily rewards.
            // Check on these that user already claimed.
            // Next avaible daily claim in?
        }

        [Command("claim"), Alias("get", "reward"), Summary("Claim your daily reward.")]
        public async Task DailyRewardsClaimAsync()
        {
            User user = User.Get(Context.User.Id);
            if (user == null)
                return;

            DateTime now = DateTime.Now;
            DateTime last = DateTime.ParseExact(user.Daily, "yyyy-MM-dd hh:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture);

            if (last == null)
            {
                user.DailyCount = 1;
                user.Daily = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                GetDailyReward(user, user.DailyCount);
                return;
            }

            int nSpan = (now - last).Minutes;
            if (nSpan >= 1440)
            {
                ++user.DailyCount;
                GetDailyReward(user, user.DailyCount);
                if (user.DailyCount >= 7)
                    user.DailyCount = 0;

                return;
            }
        }

        private void GetDailyReward(User user, int nDay)
        {
            Inventory inventory = Inventory.Get(user.UserID);
            if (inventory == null)
                return;

            switch (nDay)
            {
                case 1:
                    user.Money += Data.Singleton.MoneyRewards[0];
                    break;
                case 2:
                    user.Money += Data.Singleton.MoneyRewards[1];
                    inventory.AddItem(Data.Singleton.ItemRewards[1], 1);
                    break;
                case 3:
                    user.Money += Data.Singleton.MoneyRewards[2];
                    inventory.AddItem(Data.Singleton.ItemRewards[2], 1);
                    break;
                case 4:
                    user.Money += Data.Singleton.MoneyRewards[3];
                    inventory.AddItem(Data.Singleton.ItemRewards[3], 1);
                    break;
                case 5:
                    user.Money += Data.Singleton.MoneyRewards[4];
                    inventory.AddItem(Data.Singleton.ItemRewards[4], 1);
                    break;
                case 6:
                    user.Money += Data.Singleton.MoneyRewards[5];
                    inventory.AddItem(Data.Singleton.ItemRewards[5], 1);
                    break;
                case 7:
                    user.Money += Data.Singleton.MoneyRewards[6];
                    inventory.AddItem(Data.Singleton.ItemRewards[6], 1);
                    break;
            }
        }
    }
}