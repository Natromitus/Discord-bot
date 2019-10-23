using Discord.Commands;

using EunokiBot.Model;
using System;

namespace EunokiBot.Items
{
    [ItemID(6)]
    public class PunTicket : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            Random rand = new Random();
            DiscRefManager.Singleton.ChannelForFun.SendMessageAsync(Utilities.Puns[rand.Next(100)]);
        }
    }
}
