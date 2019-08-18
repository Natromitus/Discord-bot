using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot.Items
{
    public class ItemIDAttribute : Attribute
    {
        public int ID { get; set; }

        public ItemIDAttribute(int id)
        {
            ID = id;
        }
    }
}
