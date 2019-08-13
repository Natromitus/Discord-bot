using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Net;

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

                
                
                string sImageFileName = ImageManager.Singleton.UserInfo(Context.User, user, inventory);
                if (sImageFileName == string.Empty)
                    return;

                
                RestUserMessage picture = await channel.SendFileAsync(
                    Path.Combine(ImageManager.Singleton.FilePath, sImageFileName), string.Empty);
                string imgurl = picture.Attachments.First().Url;

                var embed = new EmbedBuilder
                {
                    /*Author = new EmbedAuthorBuilder
                    {
                        Name = "General info",
                        IconUrl = Context.Guild.IconUrl,
                    },
                    ThumbnailUrl = Context.User.GetAvatarUrl(),
                    Title = Context.User.Username,
                    Description = $"Level {user.Level}        XP: {user.XP}/{Data.Singleton.Levels[user.Level + 1].XPGap}\n" +
                    $"Money: {user.Money}",
                    Color = Color.Blue*/
                };

                embed.WithImageUrl(imgurl);

                //File.Delete(Path.Combine(ImageManager.Singleton.FilePath, sImageFileName));

                await Context.Channel.SendMessageAsync("", false, embed.Build());
                await Task.Delay(500);
                //_ = picture.DeleteAsync();
            }
        }
    }
}
