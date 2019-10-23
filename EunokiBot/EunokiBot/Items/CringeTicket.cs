using System;

using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(5)]
    public class CringeTicket : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            Random rand = new Random();
            DiscRefManager.Singleton.ChannelForFun.SendMessageAsync(Utilities.Cringe[rand.Next(100)]);
        }
    }
}
