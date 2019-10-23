using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(14)]
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
