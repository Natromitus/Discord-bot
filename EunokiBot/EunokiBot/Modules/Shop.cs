using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using EunokiBot.Model;
using EunokiBot.ImageManagment;
using Discord.Rest;
using System.IO;

namespace EunokiBot
{
    public class Shop
    {
        [Group("shop"), Summary("Group managing shop commands.")]
        public class ShopGroup : ModuleBase<SocketCommandContext>
        {
            [Command(""), Alias("show"), Summary("Display shop items.")]
            public async Task Show(int nPage = 0)
            {
                int nShopPages = Data.Singleton.ShopPages;

                if (nPage == 0)
                    nPage = 1;

                if (nPage > nShopPages)
                {
                    _ = Context.Channel.SendMessageAsync(":x: Invalid Shop page!");
                    return;
                }

                SocketTextChannel channel = Context.Guild.GetChannel(606567031730601985) as SocketTextChannel;
                if (channel == null)
                    return;

                string sImageFileName = ImageManager.Singleton.Shop(nPage);
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

            [Command("buy"), Alias("purchase"), Summary("Buy item from shop.")]
            public async Task Buy(int nID = 0, int nAmount = 0)
            {
                User user = User.Get(Context.User.Id);
                if (user == null)
                    return;

                int nCount = SQL.Singleton.GetCount("Items");
                if (nID <= 0 && nID >= nCount)
                {
                    _ = await Context.Channel.SendMessageAsync(":x: You didn't specify valid id of an item.");
                    return;
                }

                if(nAmount == 0)
                {
                    _ = await Context.Channel.SendMessageAsync(":x: You didn't specify valid amount of items you want to purchase.");
                    return;
                }

                Inventory inventory = Inventory.Get(Context.User.Id);
                if (inventory == null)
                    return;
                Item item = Item.GetItemByID(nID);
                if (item.Price * nAmount > user.Money)
                {
                    _ = await Context.Channel.SendMessageAsync(":x: Not enough money.");
                    return;
                }

                int nResult = inventory.AddItem(nID, nAmount);

                if(nResult == nAmount)
                {
                    _ = await Context.Channel.SendMessageAsync(":x: Full inventory!");
                    return;
                }

                _ = await Context.Channel.SendMessageAsync(":tada: Item was sucessfully bought!");
                user.Money -= item.Price * (nAmount - nResult);
            }
        }
    }
}
