using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using EunokiBot.Model;

namespace EunokiBot
{
    class Alerts
    {
        public async Task OnUserJoin(SocketGuildUser user)
        {
            User userModel = User.Get(user.Id);
            if (userModel == null)
            {
                User.NewRecord(new User(user.Id));
                Inventory.NewRecord(new Inventory(user.Id));
            }

            SocketTextChannel channel = DiscRefManager.Singleton.ChannelMain;

            await channel.SendMessageAsync(Utilities.GetAlert("WELCOME_&MENTION_&GUILDNAME", user.Mention, channel.Guild.Name));

            IRole role = channel.Guild.Roles.FirstOrDefault(obj => obj.Name.ToString() == "Level 1");

            IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();
            _ = dmChannel.SendMessageAsync(Utilities.GetAlert("JOINDM_&NAME", user.Mention));
            _ = user.AddRoleAsync(role);
        }

        public async Task OnLevelUp(ulong ulUserID, int nLevel)
        {
            SocketGuildUser user = DiscRefManager.Singleton.Guild.GetUser(ulUserID);
            if (user == null)
                return;

            SocketTextChannel channel = DiscRefManager.Singleton.ChannelMain;

            RemoveLevels(user, channel);
            IRole role = channel.Guild.Roles.FirstOrDefault(obj => obj.Name.ToString() == $"Level {nLevel}");
            _ = user.AddRoleAsync(role);

            if (nLevel != 1)
            {
                IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();
                _ = channel.SendMessageAsync(Utilities.GetAlert("LEVELUP_&MENTION_&LEVEL", user.Mention, nLevel));
                _ = dmChannel.SendMessageAsync(Utilities.GetAlert("LEVELUPDM_&LEVEL", nLevel));
            }
        }

        public void Minigun(ulong ulUserID)
        {
            _ = DiscRefManager.Singleton.ChannelMain.SendMessageAsync(Utilities.GetAlert(
                "CAKEDROP_&MENTION", Program.Singleton.Client.GetUser(ulUserID).Mention));
        }

        public void GumGrab(ulong ulUserID)
        {
            _ = DiscRefManager.Singleton.ChannelMain.SendMessageAsync(Utilities.GetAlert(
                "GUMGRAB_&MENTION_&AMOUNTLEFT", Program.Singleton.Client.GetUser(ulUserID).Mention, Data.Singleton.Gums));
        }

        public void CakeGrab(ulong ulUserID, bool bLie)
        {
            if(bLie)
                _ = DiscRefManager.Singleton.ChannelMain.SendMessageAsync(Utilities.GetAlert(
                    "CAKELIE_&MENTION_&AMOUNTLEFT", Program.Singleton.Client.GetUser(ulUserID).Mention, Data.Singleton.Cakes));

            else
                _ = DiscRefManager.Singleton.ChannelMain.SendMessageAsync(Utilities.GetAlert
                    ("CAKEGRAB_&MENTION_&AMOUNTLEFT", Program.Singleton.Client.GetUser(ulUserID).Mention, Data.Singleton.Cakes));
        }

        public async Task OnWarning(ulong ulUserID, int nWarnings)
        {
            SocketGuildUser user = Program.Singleton.Client.GetGuild(573131665660968970).GetUser(ulUserID);
            if (user == null)
                return;

            if (!(Program.Singleton.Client.GetChannel(603328120946163753) is SocketTextChannel channel))
                return;

            _ = await channel.SendMessageAsync($"{user.Mention} has {nWarnings} warnings!");
        }

        private void RemoveLevels(SocketGuildUser user, SocketTextChannel channel)
        {
            List<IRole> levels = new List<IRole>();
            for (int i = 1; i < 6; ++i)
                levels.Add(channel.Guild.Roles.FirstOrDefault(obj => obj.Name.ToString() == $"Level {i}"));

            user.RemoveRolesAsync(levels.AsEnumerable());
        }
    }

}
    
