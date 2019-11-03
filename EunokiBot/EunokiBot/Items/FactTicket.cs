using System;

using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(7)]
    public class FactTicket : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            Random rand = new Random();
            string sMention = context.Guild.GetUser(user.UserID).Mention;
            DiscRefManager.Singleton.ChannelForFun.SendMessageAsync(sMention + ": " + Utilities.Facts[rand.Next(100)]);
        }
    }
}
