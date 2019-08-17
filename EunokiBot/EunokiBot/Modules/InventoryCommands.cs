using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using EunokiBot.Model;

namespace EunokiBot.Modules
{
    public class InventoryCommands
    {
        [Group("inventory"), Alias("inv"), Summary("Inventory commands.")]
        public class InventoryGroup : ModuleBase<SocketCommandContext>
        {
            [Command(""), Alias("help"),Summary("List of all commands to be used with inventory.")]
            public async Task InventoryHelp()
            {
                _ = Context.Channel.SendMessageAsync("TODO: HELP TEXT");
            }


            [Command("use"), Summary("Use item on desired inventory slot.")]
            public async Task UseItem(int nIndex = 0)
            {
                IUser targetUser = null;

                if(nIndex == 0)
                {
                    _ = Context.Channel.SendMessageAsync(":x: Syntax Error: use <slot> <optional: user-mention> <optional: text>");
                    return;
                }

                if (targetUser.IsBot)
                {
                    _ = Context.Channel.SendMessageAsync(":x: No point in using this command on bot.");
                    return;
                }

                User user = User.Get(Context.User.Id);
                if (user == null)
                {
                    _ = Context.Channel.SendMessageAsync(":x: Something went wrong.");
                    return;
                }
                    

                Inventory inventory = Inventory.Get(Context.User.Id);
                if (inventory == null)
                {
                    _ = Context.Channel.SendMessageAsync(":x: Something went wrong.");
                    return;
                }

                inventory.InventoryItems[nIndex - 1].OnItemUse(Context, user, inventory, targetUser, string.Empty);
            }
        }
    }
}
