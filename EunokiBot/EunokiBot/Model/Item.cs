using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot.Model
{
    public class Item
    {
        public int ItemID { get; private set; }
        public string Name { get; private set; }
        public string Image { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }
        public int MaxStack { get; private set; }
        public int Consumable { get; private set; }
        public int Buff { get; private set; }
        public int Chance { get; private set; }

        public Item()
        {

        }
    }
}
