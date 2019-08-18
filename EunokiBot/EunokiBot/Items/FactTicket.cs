using System;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

using EunokiBot.Model;
namespace EunokiBot.Items
{
    public class FactTicket : BaseItem
    {
        [ItemID(7)]
        public override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {

        }
    }
}
