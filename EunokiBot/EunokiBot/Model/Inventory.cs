using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot.Model
{
    public class Inventory
    {
        public long SQLUser_ID { get; set; }
        public int ItemID0 { get; set; }
        public int Amount0 { get; set; }
        public int ItemID1 { get; set; }
        public int Amount1 { get; set; }
        public int ItemID2 { get; set; }
        public int Amount2 { get; set; }
        public ulong UserID
        {
            get
            {
                return (ulong)SQLUser_ID;
            }
            set
            {
                SQLUser_ID = (long)value;
            }
        }


        public Inventory(ulong id)
        {
            UserID = id;
        }

        public Inventory()
        {

        }
    }
}
