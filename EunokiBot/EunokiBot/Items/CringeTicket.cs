using System;
using System.Collections.Generic;

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
            string sMention = context.Guild.GetUser(user.UserID).Mention;
            DiscRefManager.Singleton.ChannelForFun.SendMessageAsync(sMention + ": " + Utilities.Puns[rand.Next(100)]);
        }
    }
}
