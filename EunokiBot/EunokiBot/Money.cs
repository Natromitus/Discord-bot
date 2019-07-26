using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using EunokiBot.Model;

namespace EunokiBot
{
    public class Money
    {
        [Group("money"), Summary("Group managing money commands.")]
        public class MoneyGroup : ModuleBase<SocketCommandContext>
        {
            [Command(""), Alias("me", "my"), Summary("Display info about my money.")]
            public async Task Me()
            {
                User.NewRecord(new User(Context.User.Id));
                Inventory.NewRecord(new Inventory(Context.User.Id));
            }

            [Command("give"), Summary("Used to give people stones.")]
            public async Task Give(IUser user = null, int nAmount = 0)
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

                if(nAmount == 0)
                {
                    await Context.Channel.SendMessageAsync(":x: You need to specify a valid amount of money to give.");
                    return;
                }

                SocketGuildUser gUser = Context.User as SocketGuildUser;
                if (gUser == null)
                    return;

                if(!gUser.GuildPermissions.Administrator)
                {
                    await Context.Channel.SendMessageAsync(":x: You don't have permission to use this command.");
                    return;
                }

                User modelUser = User.Get(user.Id);
                modelUser.Money += nAmount;

                await Context.Channel.SendMessageAsync($":tada: {user.Mention} you have received {nAmount} money!");
            }
        }
    }
}
