using System;
using System.Collections.Generic;
using System.Linq;

using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(16)]
    public class RaidShop : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            Random rnd = new Random();
            
            List<Item> items = Data.Singleton.Items.Where(obj => obj.Tier == 1 || obj.Tier == 2 || obj.Tier == 3).ToList();
            inventory.AddItem(items[rnd.Next(items.Count)].ItemID, 1);
        }
    }
}
