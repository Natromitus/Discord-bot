using System;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

using EunokiBot.Model;
namespace EunokiBot.Items
{
    [ItemID(27)]
    public class NoisyEater : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            // TODO: AUDIO UPDATE
        }
    }
}
