using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(1)]
    public class SmallXPCapsule : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            user.XP += 250;
            _ = context.Channel.SendMessageAsync("You have sucessfully used Small XP Capsule!");
        }
    }
}
