using System;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(20)]
    public class Overdose : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            user.XP += 1500;
            _ = context.Channel.SendMessageAsync("You have sucessfully used Overdose!");
        }
    }
}
