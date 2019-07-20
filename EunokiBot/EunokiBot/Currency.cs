using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace EunokiBot
{
    public class Currency
    {
        [Group("Money"), Summary("Group managing money commands.")]
        public class MoneyGroup : ModuleBase<SocketCommandContext>
        {
            [Command(""), Alias("me", "my"), Summary("Display info about my money.")]
            public async Task Me()
            {
                await Data.Data.SaveUser(await UserModel.CreateAsync(Context.User.Id));
            }

            [Command("give"), Summary("Used to give people stones.")]
            public async Task Give(IUser user = null, int nAmount = 0)
            {
                // give @Natromitus 50
                // $money give @Natromitus 50

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
                    await Context.Channel.SendMessageAsync(":x: You need to specify a valid amount stones to give.");
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

                await Context.Channel.SendMessageAsync($":tada: {user.Mention} you have received {nAmount} money!");
            }
        }
    }
}
