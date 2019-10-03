using System.Threading.Tasks;
using System.IO;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;

using EunokiBot.Model;
using EunokiBot.ImageManagment;

namespace EunokiBot.Modules
{
    [Group("quests"), Alias("quest"), Summary("Quests commands.")]
    public class QuestsCommands : ModuleBase<SocketCommandContext>
    {
        [Command(""), Alias("help"), Summary("List of all commands to be used with inventory.")]
        public async Task QuestsInfoAsync()
        {
            User user = User.Get(Context.User.Id);
            if (user == null)
                return;

            SocketTextChannel channel = DiscRefManager.Singleton.ChannelMain;

            string sImageFileName = ImageManager.Singleton.QuestsInfo(user);
            if (sImageFileName == string.Empty)
                return;

            RestUserMessage picture = await channel.SendFileAsync(
                Path.Combine(ImageManager.Singleton.FilePath, sImageFileName), string.Empty);
            string imgurl = picture.Attachments.First().Url;

            EmbedBuilder embed = new EmbedBuilder();
            embed.WithImageUrl(imgurl);

            File.Delete(Path.Combine(ImageManager.Singleton.FilePath, sImageFileName));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
            await Task.Delay(500);
            _ = picture.DeleteAsync();

        }

        [Command("reroll"), Alias("roll"), Summary("Generate another quests.")]
        public async Task QuestRerollAsync()
        {

        }
    }
}
