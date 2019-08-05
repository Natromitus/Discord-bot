using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using EunokiBot.Model;

namespace EunokiBot
{
    public class Shop
    {
        [Group("shop"), Summary("Group managing shop commands.")]
        public class ShopGroup : ModuleBase<SocketCommandContext>
        {
            [Command(""), Alias("show"), Summary("Display shop items.")]
            public async Task Show()
            {
                int nCount = SQL.Singleton.GetCount("Items");

                List<Item> items = new List<Item>();
                for(int i = 1; i <= nCount; ++i)
                    items.Add(Item.GetItemByID(i));

                string sTemp = "";
                foreach (Item item in items.Where(obj => obj != null))
                    sTemp += $"{item.ItemID}. {item.Name} for {item.Price} - {item.Description}\n";

                _ = await Context.Channel.SendMessageAsync($"Shop:\n" + sTemp);
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
