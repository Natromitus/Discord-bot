using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using EunokiBot.Model;

namespace EunokiBot.Modules
{
    [Group("inventory"), Alias("inv"), Summary("Inventory commands.")]
    public class InventoryCommands : ModuleBase<SocketCommandContext>
    {
        [Command(""), Alias("help"),Summary("List of all commands to be used with inventory.")]
        public async Task InventoryHelpAsync()
        {
            _ = Context.Channel.SendMessageAsync("TODO: HELP TEXT");
        }


        [Command("use"), Summary("Use item on desired inventory slot.")]
        public async Task UseItemAsync(int nIndex = 0, [Remainder]string sParam = null)
        {
            IUser targetUser = null;
            if (!string.IsNullOrWhiteSpace(sParam))
            {
                if (Context.Guild.Users.Any(obj => obj.Username == sParam))
                    targetUser = Context.Guild.Users.First(obj => obj.Username == sParam);
                else if (ulong.TryParse(sParam, out ulong targetUserId))
                {
                    if (Context.Guild.Users.Any(obj => obj.Id == targetUserId))
                        targetUser = Context.Guild.Users.First(obj => obj.Id == targetUserId);
                }
                else if (Context.Guild.Users.Any(obj => obj.Mention == sParam))
                    targetUser = Context.Guild.Users.First(obj => obj.Mention == sParam);
            }

            if (nIndex == 0)
            {
                _ = Context.Channel.SendMessageAsync(":x: Syntax Error: use <slot> <optional: user-mention> <optional: text>");
                return;
            }

            if (targetUser != null && targetUser.IsBot)
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

            if (targetUser != null)
                inventory.InventoryItems[nIndex - 1].OnItemUse(Context, user, inventory, targetUser);
            else
                inventory.InventoryItems[nIndex - 1].OnItemUse(Context, user, inventory, sParam);
        }
    }
}
