using System;

using Discord;
using Discord.Commands;

using EunokiBot.Model;
namespace EunokiBot.Items
{
    [ItemID(8)]
    public class PaperPlane : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            IUser trgUser = param as IUser;
            if(trgUser == null)
            {
                context.Channel.SendMessageAsync("Paper plane didn't have a target to hit :(");
                return;
            }

            User trgModelUser = User.Get(trgUser.Id);
            if (trgModelUser == null)
            {
                context.Channel.SendMessageAsync("Paper plane didn't have a target to hit :(");
                return;
            }

            Random rnd = new Random();
            float fRnd = (float)rnd.NextDouble();

            // 25% small hit 25% self hit %25 critical 25 nothing
            if (fRnd < 0.25f)
            {
                context.Channel.SendMessageAsync("Paper plane flew straight towards him - he just lost 25XP!");
                trgModelUser.AddXP(-25);
            }
            else if(fRnd < 0.5)
            {
                context.Channel.SendMessageAsync("Paper plane flew straight into his eye! - he just lost 50XP!");
                trgModelUser.AddXP(-50);
            }
            else if (fRnd < 0.75)
            {
                context.Channel.SendMessageAsync("Paper plane flew and after a while turned around and decided" +
                    "to turn around, hitting you. You just lost 30XP!");
                user.AddXP(-30);
            }
            else
                context.Channel.SendMessageAsync("Paper plane flew far away, far away.");
        }
    }
}
