using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using EunokiBot.Model;

namespace EunokiBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("register")]
        public async Task RegisterAsync()
        {
            User user = User.Get(Context.User.Id);

            if(user != null)
            {
                await Context.Channel.SendMessageAsync(":x: You are already registered!");
                return;
            }

            User.NewRecord(new User(Context.User.Id));
            Inventory.NewRecord(new Inventory(Context.User.Id));
            await Context.Channel.SendMessageAsync(":tada: You have been sucessfully registered!");
        }


        [Command("secret")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Secret()
        {
            if (!IsRole((SocketGuildUser)Context.User, "IT Department"))
                return;

            IDMChannel dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("SECRET_&NUMBER", 12));
        }

        public bool IsRole(SocketGuildUser user, string sRole)
        {
            var result = from role in user.Guild.Roles
                         where role.Name == sRole
                         select role.Id;

            ulong lRoleID = result.FirstOrDefault();
            if (lRoleID == 0)
                return false;

            var targetRole = user.Guild.GetRole(lRoleID);
            return user.Roles.Contains(targetRole);
        }
    }
}
