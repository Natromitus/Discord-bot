using Discord.Commands;
using Discord.WebSocket;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(15)]
    public class Minigun : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            SocketTextChannel channel = DiscRefManager.Singleton.ChannelMain;
            _ = channel.SendMessageAsync(Utilities.GetAlert(
                "MINIGUN_&MENTION", Program.Singleton.Client.GetUser(user.UserID).Mention));
            _ = channel.SendMessageAsync("Pew");
            _ = channel.SendMessageAsync("PEW!");
            _ = channel.SendMessageAsync("pew?");
            _ = channel.SendMessageAsync("PeW!");
        }
    }
}
