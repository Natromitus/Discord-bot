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
                User.NewRecord(new User(user.Id));

            var channel = Program.Singleton.Client.GetChannel(573131665660968972) as SocketTextChannel;
            if (channel == null)
                return;

            await channel.SendMessageAsync(Utilities.GetAlert("WELCOME_&MENTION_&GUILDNAME", user.Mention, channel.Guild.Name));
            SocketGuild guild = Program.Singleton.Client.GetGuild(573131665660968970) as SocketGuild;
            if (guild == null)
                return;

            IRole role = channel.Guild.Roles.FirstOrDefault(obj => obj.Name.ToString() == "Level 1");

            IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("JOINDM_&NAME", user.Mention));

            await user.AddRoleAsync(role);
        }

        public async Task OnLevelUp(ulong ulUserID, int nLevel)
        {
            SocketGuildUser user = Program.Singleton.Client.GetGuild(573131665660968970).GetUser(ulUserID);
            if (user == null)
                return;

            SocketTextChannel channel = Program.Singleton.Client.GetChannel(573131665660968972) as SocketTextChannel;
            if (channel == null)
                return;

            IRole role = channel.Guild.Roles.FirstOrDefault(obj => obj.Name.ToString() == $"Level {nLevel}");
            RemoveLevels(user, channel);
             await user.AddRoleAsync(role);

            _ = await channel.SendMessageAsync($"{user.Mention} has just reached {nLevel} level!");
        }

        public async Task OnWarning(ulong ulUserID, int nWarnings)
        {
            SocketGuildUser user = Program.Singleton.Client.GetGuild(573131665660968970).GetUser(ulUserID);
            if (user == null)
                return;

            SocketTextChannel channel = Program.Singleton.Client.GetChannel(603328120946163753) as SocketTextChannel;
            if (channel == null)
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
