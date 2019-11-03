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
        [Group("shop"), Alias("item", "items", "itm", "shp", "market"), Summary("Group managing shop commands.")]
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

                string sImageFileName = ImageManager.Singleton.Shop(nPage);
                if (sImageFileName == string.Empty)
                    return;

                _ = Context.Channel.SendFileAsync(
                    Path.Combine(ImageManager.Singleton.FilePath, sImageFileName), string.Empty);
            }

            [Command("buy"), Alias("purchase"), Summary("Buy item from shop.")]
            public async Task Buy(int nID = 0, int nAmount = 0)
            {
                User user = User.Get(Context.User.Id);
                if (user == null)
                    return;

                if (nID <= 0 && nID >= Data.Singleton.Items.Count)
                {
                    _ = await Context.Channel.SendMessageAsync(":x: You didn't specify valid id of an item.");
                    return;
                }

                if (nAmount == 0)
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

                if (nResult == nAmount)
                {
                    _ = await Context.Channel.SendMessageAsync(":x: Full inventory!");
                    return;
                }

                user.Money -= item.Price * (nAmount - nResult);

                if (user.Notifications == 0)
                    return;

                IDMChannel dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                string sImagePath = Path.Combine(ImageManager.Singleton.FilePath, 
                    ImageManager.Singleton.ItemBought(nID, nAmount - nResult));
                await dmChannel.SendFileAsync(sImagePath);
                File.Delete(sImagePath);
            }

            [Command("info"), Alias("desc", "description", "detail"), Summary("Information about item.")]
            public async Task Info(int nID = -1)
            {
                if(nID < 0 || nID > Data.Singleton.Items.Count)
                {
                    _ = await Context.Channel.SendMessageAsync(":x: Invalid Item ID");
                    return;
                }

                string sImageFileName = ImageManager.Singleton.ItemDesc(nID);
                if (sImageFileName == string.Empty)
                    return;

                RestUserMessage picture = await Context.Channel.SendFileAsync(
                    Path.Combine(ImageManager.Singleton.FilePath, sImageFileName), string.Empty);

                File.Delete(Path.Combine(ImageManager.Singleton.FilePath, sImageFileName));
            }
        }
    }
}
