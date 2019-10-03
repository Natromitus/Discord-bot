using System;
using System.Collections.Generic;
using System.IO;
using Discord;
using Discord.Commands;
using EunokiBot.ImageManagment;
using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(12)]
    public class RicardoTime : BaseItem
    {
        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            int nCount = ImageManager.Singleton.RicardoFiles.Length;

            Random rnd = new Random();
            int nRandom = rnd.Next(nCount);

            string sMention= context.Guild.GetUser(user.UserID).Mention;

            DiscRefManager.Singleton.ChannelMain.SendFileAsync(
                Path.Combine(ImageManager.Singleton.FilePath, "Ricardo", $"{nRandom}.gif"),
               sMention + "has just used Ricardo Time!");
        }
    }
}
