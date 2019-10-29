using System.Threading.Tasks;
using System.IO;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;

using EunokiBot.Model;
using EunokiBot.ImageManagment;
using System;

namespace EunokiBot.Modules
{
    [Group("quests"), Alias("quest"), Summary("Quests commands.")]
    public class QuestsCommands : ModuleBase<SocketCommandContext>
    {
        [Command(""), Alias("info"), Summary("List of all quests.")]
        public async Task QuestsInfoAsync()
        {
            User user = User.Get(Context.User.Id);
            if (user == null)
                return;

            SocketTextChannel channel = DiscRefManager.Singleton.ChannelMain;

            string sImageFileName = ImageManager.Singleton.QuestsInfo(user);
            if (sImageFileName == string.Empty)
                return;

            await Context.Channel.SendFileAsync(Path.Combine(ImageManager.Singleton.FilePath, sImageFileName));
            File.Delete(Path.Combine(ImageManager.Singleton.FilePath, sImageFileName));
        }

        [Command("reroll"), Alias("roll"), Summary("Assign new daily quests to user.")]
        public async Task QuestRerollAsync()
        {
            User user = User.Get(Context.User.Id);
            if (user == null)
                return;

            IDMChannel dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            DateTime now = DateTime.Now;
            DateTime last = DateTime.ParseExact(user.Reroll, "yyyy-MM-dd HH:mm:ss", 
                System.Globalization.CultureInfo.InvariantCulture);

            TimeSpan span = now - last;
            if (span.TotalMinutes < 1440)
            {
                _ = dmChannel.SendMessageAsync(Utilities.GetAlert("QUESTSREROLL_COOLDOWN"));
                return;
            }

            user.AssignQuests();
            user.Reroll = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _ = dmChannel.SendMessageAsync(Utilities.GetAlert("QUESTSREROLL_EXECUTED"));
        }
    }
}
