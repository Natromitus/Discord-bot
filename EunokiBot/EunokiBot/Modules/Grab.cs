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
    public class Grab : ModuleBase<SocketCommandContext>
    {
        [Command("grab gum"), Summary("Grab gum.")]
        public async Task GrabGumAsync()
        {
            if (Data.Singleton.Gums <= 0)
                return;

            User user = User.Get(Context.User.Id);
            user.AddXP(83);
            --Data.Singleton.Gums;
            Program.Singleton.AlertsHandler.GumGrab(Context.User.Id);
        }

        [Command("grab cake"), Summary("Grab cake.")]
        public async Task GrabCakeAsync()
        {
            if (Data.Singleton.Cakes <= 0)
                return;

            User user = User.Get(Context.User.Id);

            Random rnd = new Random();
            float fRnd = (float)rnd.NextDouble();

            if (fRnd < 0.5f)
            {
                user.AddXP(250);
                --Data.Singleton.Cakes;
                Program.Singleton.AlertsHandler.CakeGrab(Context.User.Id, false);
            }
            else
            {
                user.AddXP(-250);
                --Data.Singleton.Cakes;
                Program.Singleton.AlertsHandler.CakeGrab(Context.User.Id, true);
            }
        }
    }
}
