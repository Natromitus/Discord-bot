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

                string sImageFileName = ImageManager.Singleton.UserInfo(Context.User, user, inventory);
                if (sImageFileName == string.Empty)
                    return;
                
                RestUserMessage picture = await Context.Channel.SendFileAsync(
                    Path.Combine(ImageManager.Singleton.FilePath, sImageFileName), string.Empty);

                File.Delete(Path.Combine(ImageManager.Singleton.FilePath, sImageFileName));
            }
        }
    }
}
