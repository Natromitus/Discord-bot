using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(16)]
    public class RaidShop : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            
        }
    }
}
