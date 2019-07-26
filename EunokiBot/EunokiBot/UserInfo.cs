using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using EunokiBot.Model;

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
                for(int i = 0; i <= 2; ++i)
                {
                    embed.AddField(new EmbedFieldBuilder()
                    {
                        IsInline = true,
                        Name = Item.GetItemByID(inventory.GetID(i)).Name + " x" + inventory.GetAmount(i),
                        Value = Item.GetItemByID(inventory.GetID(i)).Description
                    });
                }

                _ = Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }
    }
}
