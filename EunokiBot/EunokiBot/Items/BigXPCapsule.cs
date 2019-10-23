using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(12)]
    public class BigXPCapsule : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            user.XP += 1000;
            _ = context.Channel.SendMessageAsync("You have sucessfully used Big XP Capsule!");
        }
    }
}
