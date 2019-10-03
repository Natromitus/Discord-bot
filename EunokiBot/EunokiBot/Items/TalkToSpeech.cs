using System;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(31)]
    public class TalkToSpeech : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            _ = DiscRefManager.Singleton.ChannelMain.SendMessageAsync(param.ToString(), true);
        }
    }
}
