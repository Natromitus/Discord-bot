using System;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(2)]
    public class ChewingGum : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            Data.Singleton.Gums += 3;
            _ = DiscRefManager.Singleton.ChannelMain.SendMessageAsync(Utilities.GetAlert(
                "GUMDROP_&MENTION", Program.Singleton.Client.GetUser(user.UserID).Mention));
        }
    }
}
