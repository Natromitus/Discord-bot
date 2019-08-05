using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;

using EunokiBot.Model;
using EunokiBot.ImageManagment;

namespace EunokiBot
{
    public class UserInfo
    {
        [Group("me"), Summary("Info about the user.")]
        public class ShopGroup : ModuleBase<SocketCommandContext>
        {
            [Command(""), Alias("info", "show", "general"), Summary("Display user's info.")]
            public async Task General()
            {
                User user = User.Get(Context.User.Id);
                if (user == null)
                    return;

                Inventory inventory = Inventory.Get(Context.User.Id);
                if (inventory == null)
                    return;

                SocketTextChannel channel = Context.Guild.GetChannel(606567031730601985) as SocketTextChannel;
                if (channel == null)
                    return;

                int[] arIDs = new int[]
                {
                    inventory.ItemID1, inventory.ItemID2, inventory.ItemID3,
                    inventory.ItemID4, inventory.ItemID5, inventory.ItemID6,
                    inventory.ItemID7, inventory.ItemID8, inventory.ItemID9
                };

                int[] arAmounts = new int[]
                {
                    inventory.Amount1, inventory.Amount2, inventory.Amount3,
                    inventory.Amount4, inventory.Amount5, inventory.Amount6,
                    inventory.Amount7, inventory.Amount8, inventory.Amount9
                };

                string sImageFileName = ImageManager.Singleton.CreateInventoryImage(arIDs, arAmounts);
                if (sImageFileName == string.Empty)
                    return;

                RestUserMessage picture = await channel.SendFileAsync(
                    Path.Combine(ImageManager.Singleton.FilePath, sImageFileName), string.Empty);
                string imgurl = picture.Attachments.First().Url;

                var embed = new EmbedBuilder
                {
                    Author = new EmbedAuthorBuilder
                    {
                        Name = "General info",
                        IconUrl = Context.Guild.IconUrl,
                    },
                    ThumbnailUrl = Context.User.GetAvatarUrl(),
                    Title = Context.User.Username,
                    Description = $"Level {user.Level}        XP: {user.XP}/{SQL.Singleton.GetLevelGapByIndex(user.Level + 1)}\n" +
                    $"Money: {user.Money}",
                    Color = Color.Blue
                };

                embed.WithImageUrl(imgurl);

                File.Delete(Path.Combine(ImageManager.Singleton.FilePath, sImageFileName));

                await Context.Channel.SendMessageAsync("", false, embed.Build());
                await Task.Delay(500);
                _ = picture.DeleteAsync();
            }
        }
    }
}
