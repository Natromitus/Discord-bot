using System;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

using EunokiBot.Model;
namespace EunokiBot.Items
{
    [ItemID(8)]
    public class PaperPlane : BaseItem
    {
        public override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {

        }
    }
}
