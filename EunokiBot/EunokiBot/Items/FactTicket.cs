using System;

using Discord.Commands;

using EunokiBot.Model;
namespace EunokiBot.Items
{
    public class FactTicket : BaseItem
    {
        [ItemID(7)]
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            Random rand = new Random();
            DiscRefManager.Singleton.ChannelForFun.SendMessageAsync(Utilities.Facts[rand.Next(100)]);
        }
    }
}
