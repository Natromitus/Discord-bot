using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq;

using Dapper;

using EunokiBot.Items;

namespace EunokiBot.Model
{
    public class Inventory : Root
    {
        #region Fields
        public const string TABLE_NAME = "Inventory";
        public const string PRIMARY_KEY = "UserID";

        private BaseItem[] m_arInventoryItems = null;

        // Item on slot
        private int m_nItemID1;
        private int m_nItemID2;
        private int m_nItemID3;
        private int m_nItemID4;
        private int m_nItemID5;
        private int m_nItemID6;
        private int m_nItemID7;
        private int m_nItemID8;

        // Amount of items on the slot
        private int m_nAmount1;
        private int m_nAmount2;
        private int m_nAmount3;
        private int m_nAmount4;
        private int m_nAmount5;
        private int m_nAmount6;
        private int m_nAmount7;
        private int m_nAmount8;
        #endregion

        #region Properties
        public long SQLUser_ID { get; set; }
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

        public BaseItem[] InventoryItems
        {
            get
            {
                if(m_arInventoryItems == null)
                {
                    m_arInventoryItems = new BaseItem[8];
                    for (int i = 0; i < m_arInventoryItems.Count(); ++i)
                        m_arInventoryItems[i] = GetItemClass(GetID(i));
                }

                return m_arInventoryItems;
            }
        }

        private IEnumerable<KeyValuePair<int, int>> ItemIDs
        {
            get
            {
                yield return new KeyValuePair<int, int>(ItemID1, Amount1);
                yield return new KeyValuePair<int, int>(ItemID2, Amount2);
                yield return new KeyValuePair<int, int>(ItemID3, Amount3);
                yield return new KeyValuePair<int, int>(ItemID4, Amount4);
                yield return new KeyValuePair<int, int>(ItemID5, Amount5);
                yield return new KeyValuePair<int, int>(ItemID6, Amount6);
                yield return new KeyValuePair<int, int>(ItemID7, Amount7);
                yield return new KeyValuePair<int, int>(ItemID8, Amount8);
            }
        }

        #region Slots
        #region Slot1
        public int ItemID1
        {
            get { return m_nItemID1; }
            set
            {
                SetField(ref m_nItemID1, value);
            }
        }
        public int Amount1
        {
            get { return m_nAmount1; }
            set
            {
                SetField(ref m_nAmount1, value);
            }
        }
        #endregion
        #region Slot2
        public int ItemID2
        {
            get { return m_nItemID2; }
            set
            {
                SetField(ref m_nItemID2, value);
            }
        }
        public int Amount2
        {
            get { return m_nAmount2; }
            set
            {
                SetField(ref m_nAmount2, value);
            }
        }
        #endregion
        #region Slot3
        public int ItemID3
        {
            get { return m_nItemID3; }
            set
            {
                SetField(ref m_nItemID3, value);
            }
        }
        public int Amount3
        {
            get { return m_nAmount3; }
            set
            {
                SetField(ref m_nAmount3, value);
            }
        }
        #endregion
        #region Slot4
        public int ItemID4
        {
            get { return m_nItemID4; }
            set
            {
                SetField(ref m_nItemID4, value);
            }
        }
        public int Amount4
        {
            get { return m_nAmount4; }
            set
            {
                SetField(ref m_nAmount4, value);
            }
        }
        #endregion
        #region Slot5
        public int ItemID5
        {
            get { return m_nItemID5; }
            set
            {
                SetField(ref m_nItemID5, value);
            }
        }
        public int Amount5
        {
            get { return m_nAmount5; }
            set
            {
                SetField(ref m_nAmount5, value);
            }
        }
        #endregion
        #region Slot6
        public int ItemID6
        {
            get { return m_nItemID6; }
            set
            {
                SetField(ref m_nItemID6, value);
            }
        }
        public int Amount6
        {
            get { return m_nAmount6; }
            set
            {
                SetField(ref m_nAmount6, value);
            }
        }
        #endregion
        #region Slot7
        public int ItemID7
        {
            get { return m_nItemID7; }
            set
            {
                SetField(ref m_nItemID7, value);
            }
        }
        
        public int Amount7
        {
            get { return m_nAmount7; }
            set
            {
                SetField(ref m_nAmount7, value);
            }
        }
        #endregion
        #region Slot8
        public int ItemID8
        {
            get { return m_nItemID8; }
            set
            {
                SetField(ref m_nItemID8, value);
            }
        }
        public int Amount8
        {
            get { return m_nAmount8; }
            set
            {
                SetField(ref m_nAmount8, value);
            }
        }
        #endregion
        #endregion
        #endregion

        public Inventory(ulong id)
        {
            UserID = id;
            ItemID1 = ItemID2 = ItemID3 = Amount1 = Amount2 = Amount3 =
            ItemID4 = ItemID5 = ItemID6 = Amount4 = Amount5 = Amount6 =
            ItemID7 = ItemID8 = Amount7 = Amount8 = 0;
        }

        protected Inventory()
        {
        }

        public int GetAmount (int nIndex)
        {
            switch(nIndex)
            {
                case 0:
                    return m_nAmount1;
                case 1:
                    return m_nAmount2;
                case 2:
                    return m_nAmount3;
                case 3:
                    return m_nAmount4;
                case 4:
                    return m_nAmount5;
                case 5:
                    return m_nAmount6;
                case 6:
                    return m_nAmount7;
                case 7:
                    return m_nAmount8;
                default:
                    return 0;
            }
        }

        public int GetID(int nIndex)
        {
            switch (nIndex)
            {
                case 0:
                    return m_nItemID1;
                case 1:
                    return m_nItemID2;
                case 2:
                    return m_nItemID3;
                case 3:
                    return m_nItemID4;
                case 4:
                    return m_nItemID5;
                case 5:
                    return m_nItemID6;
                case 6:
                    return m_nItemID7;
                case 7:
                    return m_nItemID8;
                default:
                    return 0;
            }
        }

        public static void NewRecord(Inventory inventory)
        {
            SQL.Singleton.Connection.Execute($"INSERT INTO {TABLE_NAME}" +
                $"({PRIMARY_KEY}," +
                " ItemID1, Amount1, ItemID2, Amount2, ItemID3, Amount3," +
                " ItemID4, Amount4, ItemID5, Amount5, ItemID6, Amount6," +
                " ItemID7, Amount7, ItemID8, Amount8)" +
                $" VALUES (@{PRIMARY_KEY}," +
                " @ItemID1, @Amount1, @ItemID2, @Amount2, @ItemID3, @Amount3," +
                " @ItemID4, @Amount4, @ItemID5, @Amount5, @ItemID6, @Amount6," +
                " @ItemID7, @Amount7, @ItemID8, @Amount8)", inventory);
        }

        public int AddItem(int nID, int nAmount)
        {
            Item item = Item.GetItemByID(nID);

            while (nAmount != 0)
            {
                int pos = ItemIDs.ToList().FindIndex(obj => obj.Key == nID && item.MaxStack > obj.Value);
                if (pos < 0)
                    pos = ItemIDs.ToList().FindIndex(obj => obj.Key == 0);

                if (pos < 0)
                    return nAmount;

                int nTemp = item.MaxStack - GetAmount(pos);
                if (nAmount > nTemp)
                {
                    SetItemAt(pos, nID, item.MaxStack);
                    nAmount = nAmount - nTemp;
                }
                else
                {
                    SetItemAt(pos, nID, GetAmount(pos) + nAmount);
                    nAmount = 0;
                }
            }

            return 0;
        }

        public void RemoveItem(int nID)
        {
            // Get index of slot with least amount of desired item

            // Need to find index of slot that has same ID but has the least amount of it.
            /*
            int pos = ItemIDs.ToList().FindAll(obj => obj.Key == nID).Select(
                obj => obj.Value).ToList().FindIndex(obj => obj.Aggregate(
                (minItem, nextItem) => minItem < nextItem ? minItem : nextItem));

            int nAmount = GetAmount(pos) - 1;
            if (nAmount == 0)
                nID = 0;

            SetItemAt(pos, nID, nAmount);*/

        }

        public void SetItemAt(int nIndex, int nID, int nAmount)
        {
            switch(nIndex)
            {
                case 0:
                    ItemID1 = nID;
                    Amount1 = nAmount;
                    break;
                case 1:
                    ItemID2 = nID;
                    Amount2 = nAmount;
                    break;
                case 2:
                    ItemID3 = nID;
                    Amount3 = nAmount;
                    break;
                case 3:
                    ItemID4 = nID;
                    Amount4 = nAmount;
                    break;
                case 4:
                    ItemID5 = nID;
                    Amount5 = nAmount;
                    break;
                case 5:
                    ItemID6 = nID;
                    Amount6 = nAmount;
                    break;
                case 6:
                    ItemID7 = nID;
                    Amount7 = nAmount;
                    break;
                case 7:
                    ItemID8 = nID;
                    Amount8 = nAmount;
                    break;
                default:
                    return;
            }
        }

        public static Inventory Get(ulong ulUserID)
        {
            using (WriteSuspender wSus = new WriteSuspender())
            using (ReadSuspender rSus = new ReadSuspender())
                return SQL.Singleton.GetValue<Inventory>(TABLE_NAME, "*", PRIMARY_KEY, (long)ulUserID);
        }

        private BaseItem GetItemClass(int nID)
        {
            foreach(TypeInfo iter in Data.Singleton.ItemTypes)
            {
                ItemIDAttribute attr = iter.GetCustomAttribute<ItemIDAttribute>();
                if (attr == null)
                    continue;

                if (attr.ID == nID)
                    return (BaseItem)Activator.CreateInstance(iter);
            }

            return null;
        }

        protected override string OnGetTableName() => TABLE_NAME;

        protected override string OnGetPrimaryKeyName() => PRIMARY_KEY;

        protected override object OnGetPrimaryKeyValue() => SQLUser_ID;
    }
}
