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
            string sMention = context.Guild.GetUser(user.UserID).Mention;
            DiscRefManager.Singleton.ChannelForFun.SendMessageAsync(sMention + ": " + Utilities.Puns[rand.Next(100)]);
        }
    }
}
