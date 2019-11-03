using System;
using System.Linq;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    [ItemID(13)]
    public class MysteryBox : BaseItem
    {
        float fTier1 = 0.80f;
        float fTier2 = 0.16f;
        float fTier3 = 0.025f;
        float fTier4 = 0.01f;
        float fTier5 = 0.005f;

        protected override void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            Random rnd = new Random();
            float fRnd = (float)rnd.NextDouble();

            if (fRnd < fTier1)
            {
                List<Item> items = Data.Singleton.Items.Where(obj => obj.Tier == 1).ToList();
                Random rnd2 = new Random();
                inventory.AddItem(items[rnd.Next(items.Count)].ItemID, 3);
            }
            else if (fRnd < fTier1 + fTier2)
            {
                List<Item> items = Data.Singleton.Items.Where(obj => obj.Tier == 2).ToList();
                Random rnd2 = new Random();
                inventory.AddItem(items[rnd.Next(items.Count)].ItemID, 2);
            }
            else if (fRnd < fTier1 + fTier2 + fTier3)
            {
                List<Item> items = Data.Singleton.Items.Where(obj => obj.Tier == 3).ToList();
                Random rnd2 = new Random();
                inventory.AddItem(items[rnd.Next(items.Count)].ItemID, 1);
            }
            else if (fRnd < fTier1 + fTier2 + fTier3 + fTier4)
            {
                List<Item> items = Data.Singleton.Items.Where(obj => obj.Tier == 4).ToList();
                Random rnd2 = new Random();
                inventory.AddItem(items[rnd.Next(items.Count)].ItemID, 1);
            }
            else if (fRnd < fTier1 + fTier2 + fTier3 + fTier4 + fTier5)
            {
                List<Item> items = Data.Singleton.Items.Where(obj => obj.Tier == 5).ToList();
                Random rnd2 = new Random();
                inventory.AddItem(items[rnd.Next(items.Count)].ItemID, 1);
            }
        }
    }
}
