using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(3)]
    public class Cake : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            ++Data.Singleton.Cakes;
            _ = DiscRefManager.Singleton.ChannelMain.SendMessageAsync(Utilities.GetAlert(
                "CAKEDROP_&MENTION", Program.Singleton.Client.GetUser(user.UserID).Mention));
        }
    }
}
