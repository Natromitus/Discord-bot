using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Commands;

using EunokiBot.Model;
using EunokiBot.Quests;
using System;
using System.IO;
using EunokiBot.ImageManagment;

namespace EunokiBot.Modules
{
    [Group("inventory"), Alias("inv", "i"), Summary("Inventory commands.")]
    public class InventoryCommands : ModuleBase<SocketCommandContext>
    {
        [Command(""), Alias("help"),Summary("List of all commands to be used with inventory.")]
        public async Task InventoryHelpAsync()
        {
            _ = Context.Channel.SendMessageAsync("TODO: HELP TEXT");
        }


        [Command("use"), Summary("Use item on desired inventory slot.")]
        public async Task UseItemAsync(int nIndex = 0, int nAmount = 1, [Remainder]string sParam = null)
        {
            // Mention Check
            IUser targetUser = null;
            if (!string.IsNullOrWhiteSpace(sParam))
            {
                string sMentionParam = "<@!" + sParam.Substring(2); 

                if (Context.Guild.Users.Any(obj => obj.Username == sParam))
                    targetUser = Context.Guild.Users.First(obj => obj.Username == sParam);
                else if (ulong.TryParse(sParam, out ulong targetUserId))
                {
                    if (Context.Guild.Users.Any(obj => obj.Id == targetUserId))
                        targetUser = Context.Guild.Users.First(obj => obj.Id == targetUserId);
                }
                else if (Context.Guild.Users.Any(obj => obj.Mention == sMentionParam))
                    targetUser = Context.Guild.Users.First(obj => obj.Mention == sMentionParam);
            }

            if (nIndex == 0)
            {
                _ = Context.Channel.SendMessageAsync(":x: Syntax Error: use <slot> <amount> <optional: user-mention> <optional: text>");
                return;
            }

            if(nAmount < 0 )
            {
                _ = Context.Channel.SendMessageAsync(":x: Syntax Error: use <slot> <amount> <optional: user-mention> <optional: text>");
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

            if(inventory.GetAmount(nIndex - 1) < nAmount)
            {
                _ = Context.Channel.SendMessageAsync(":x: Not enough items!");
                return;
            }

            // Quests
            ActionParam action = new ActionParam("Item", Convert.ToUInt64(inventory.GetID(nIndex - 1)));
            for(int i = 0; i < nAmount; ++i)
                ActionManager.Singleton.OnAction(user, action);

            for(int i = 0; i < nAmount; ++i)
            {
                if (targetUser != null)
                    inventory.InventoryItems[nIndex - 1].Use(Context, user, inventory, targetUser);
                else
                    inventory.InventoryItems[nIndex - 1].Use(Context, user, inventory, sParam);
            }

            // If Cake or Gum send Alert
            if(inventory.GetID(nIndex - 1) == 3)
            {
                _ = DiscRefManager.Singleton.ChannelMain.SendMessageAsync(Utilities.GetAlert(
                    "CAKEDROP_&MENTION", Program.Singleton.Client.GetUser(user.UserID).Mention) +
                    " x" + nAmount);
            }
            else if (inventory.GetID(nIndex - 1) == 2)
            {
                _ = DiscRefManager.Singleton.ChannelMain.SendMessageAsync(Utilities.GetAlert(
                    "GUMDROP_&MENTION", Program.Singleton.Client.GetUser(user.UserID).Mention) +
                    " x" + nAmount);
            }

            // Send DM of Item used
            IDMChannel dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            string sImagePath = Path.Combine(ImageManager.Singleton.FilePath,
                ImageManager.Singleton.ItemUsed(inventory.GetID(nIndex - 1), nAmount));
            await dmChannel.SendFileAsync(sImagePath);
            File.Delete(sImagePath);
        }
    }
}
