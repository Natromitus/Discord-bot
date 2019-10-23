using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(17)]
    public class MightySlap : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            DiscRefManager.Singleton.ChannelMain.SendMessageAsync(context.User.Mention + " has just slapped mods!");
        }
    }
}
