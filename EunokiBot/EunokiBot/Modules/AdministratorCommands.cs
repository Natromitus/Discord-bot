using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using EunokiBot.ImageManagment;
using EunokiBot.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EunokiBot.Modules
{
    public class Administrator : ModuleBase<SocketCommandContext>
    {
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [Command("warning"), Alias("warn"), Summary("Warns the player.")]
        public async Task WarningAsync(IUser iUser)
        {
            if (iUser == null)
            {
                await Context.Channel.SendMessageAsync(":x: Syntax error: warning <user>");
                return;
            }

            if (iUser.IsBot)
            {
                await Context.Channel.SendMessageAsync(":x: No point in using this command on bot.");
                return;
            }

            User user = User.Get(iUser.Id);
            if (user == null)
                return;

            ++user.Warnings;
            if (user.Notifications == 0)
                return;

            IDMChannel dmChannel = await iUser.GetOrCreateDMChannelAsync();
            _ = dmChannel.SendMessageAsync("You have been warned by moderator!");
        }

        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [Command("ban"), Summary("Bans the player.")]
        public async Task BanAsync(IGuildUser user, int days = 0, string reason = "No reason provided.")
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync(":x: Syntax error: ban <user> <prune days> <reason>");
                return;
            }

            if (user.IsBot)
            {
                await Context.Channel.SendMessageAsync(":x: No point in using this command on bot.");
                return;
            }

            _ = user.Guild.AddBanAsync(user, days, reason);
            _ = user.Guild.RemoveBanAsync(user);
        }

        [RequireBotPermission(GuildPermission.KickMembers)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [Command("kick"), Summary("Kicks the player.")]
        public async Task KickAsync(IGuildUser user, string reason = "No reason provided.")
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync(":x: Syntax error: kick <user> <reason>");
                return;
            }

            if (user.IsBot)
            {
                await Context.Channel.SendMessageAsync(":x: No point in using this command on bot.");
                return;
            }

            _ = user.KickAsync(reason);
        }

        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("user"), Summary("Gives user's stats.")]
        public async Task UserStatsAsync(SocketUser iUser)
        {
            User user = User.Get(iUser.Id);
            if (user == null)
                return;

            Inventory inventory = Inventory.Get(iUser.Id);
            if (inventory == null)
                return;

            string sImageFileName = ImageManager.Singleton.UserInfo(iUser, user, inventory);
            if (sImageFileName == string.Empty)
                return;

            await Context.Channel.SendFileAsync(
                Path.Combine(ImageManager.Singleton.FilePath, sImageFileName), string.Empty);

            File.Delete(Path.Combine(ImageManager.Singleton.FilePath, sImageFileName));
        }

        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("money give"), Summary("Gives user money.")]
        public async Task GiveMoney(IUser user, int nAmount = 0)
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync(":x: Syntax error: money give <user> <amount>");
                return;
            }

            if (user.IsBot)
            {
                await Context.Channel.SendMessageAsync(":x: No point in using this command on bot.");
                return;
            }

            if (nAmount == 0)
            {
                await Context.Channel.SendMessageAsync(":x: You need to specify a valid amount of money to give.");
                return;
            }

            User modelUser = User.Get(user.Id);
            modelUser.Money += nAmount;

            if (modelUser.Notifications == 0)
                return;

            IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();
            if(nAmount > 0)
                _ = dmChannel.SendMessageAsync($":tada: {user.Mention} you have received {nAmount} money!");
            else
                _ = dmChannel.SendMessageAsync($":x: {Context.User.Mention} took {nAmount} money from you!");
        }

        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("item give"), Summary("Gives user item of choice..")]
        public async Task GiveItem(IUser user, int nID = 0, int nAmount = 0)
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync(":x: Syntax error: item give <user> <item-id> <amount>");
                return;
            }

            if (user.IsBot)
            {
                await Context.Channel.SendMessageAsync(":x: No point in using this command on bot.");
                return;
            }

            if (nID == 0)
            {
                await Context.Channel.SendMessageAsync(":x: You need to specify a valid item to give.");
                return;
            }

            if (nAmount == 0)
            {
                await Context.Channel.SendMessageAsync(":x: You need to specify a valid amount of items to give.");
                return;
            }

            Inventory inventory = Inventory.Get(user.Id);
            if (inventory == null)
                return;

            inventory.AddItem(nID, nAmount);

            User modelUser = User.Get(user.Id);
            if (modelUser == null)
                return;

            if (modelUser.Notifications == 0)
                return;

            IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();
            _ = dmChannel.SendMessageAsync($":tada: {user.Mention} you have received some items!");
        }

        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("inventory clear"), Summary("Clears user's inventory.")]
        public async Task InventoryClearAsync(IUser user)
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync(":x: Syntax error: inventory clear <user>");
                return;
            }

            if (user.IsBot)
            {
                await Context.Channel.SendMessageAsync(":x: No point in using this command on bot.");
                return;
            }

            Inventory inventory = Inventory.Get(user.Id);
            if (inventory == null)
                return;

            for (int i = 0; i < 8; ++i)
                inventory.SetItemAt(i, 0, 0);

            User modelUser = User.Get(user.Id);
            if (modelUser == null)
                return;

            if (modelUser.Notifications == 0)
                return;

            IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();
            _ = dmChannel.SendMessageAsync($":x: {Context.User.Mention} cleared your inventory.");
        }

        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("inventory clearslot"), Summary("Clears user's inventory slot.")]
        public async Task InventoryClearAsync(IUser user, int nIndex = 0)
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync(":x: Syntax error: item give <user> <item-id> <amount>");
                return;
            }

            if (user.IsBot)
            {
                await Context.Channel.SendMessageAsync(":x: No point in using this command on bot.");
                return;
            }

            if(nIndex == 0)
            {
                await Context.Channel.SendMessageAsync(":x: You need to specify a valid inventory slot to clear. (1 - 8)");
                return;
            }

            Inventory inventory = Inventory.Get(user.Id);
            if (inventory == null)
                return;

            inventory.SetItemAt(nIndex - 1, 0, 0);

            User modelUser = User.Get(user.Id);
            if (modelUser == null)
                return;

            if (modelUser.Notifications == 0)
                return;

            IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();
            _ = dmChannel.SendMessageAsync($":x: {Context.User.Mention} cleared your {nIndex}. inventory slot!");
        }
    }
}
