using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace EunokiBot.Model
{
    public class Inventory : Root
    {
        #region Fields
        private const string m_sTableName = "Inventory";
        private const string m_sPrimaryKey = "UserID";
        private int m_nItemID1;
        private int m_nItemID2;
        private int m_nItemID3;
        private int m_nAmount1;
        private int m_nAmount2;
        private int m_nAmount3;
        #endregion

        #region Properties
        public long SQLUser_ID { get; set; }
        public int ItemID1
        {
            get { return m_nItemID1; }
            set
            {
                SetField<int>(ref m_nItemID1, value);
            }
        }
        public int Amount1
        {
            get { return m_nAmount1; }
            set
            {
                SetField<int>(ref m_nAmount1, value);
            }
        }
        public int ItemID2
        {
            get { return m_nItemID2; }
            set
            {
                SetField<int>(ref m_nItemID2, value);
            }
        }
        public int Amount2
        {
            get { return m_nAmount2; }
            set
            {
                SetField<int>(ref m_nAmount2, value);
            }
        }

        public int ItemID3
        {
            get { return m_nItemID3; }
            set
            {
                SetField<int>(ref m_nItemID3, value);
            }
        }
        public int Amount3
        {
            get { return m_nAmount3; }
            set
            {
                SetField<int>(ref m_nAmount3, value);
            }
        }

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
        private IEnumerable<KeyValuePair<int, int>> ItemIDs
        {
            get
            {
                yield return new KeyValuePair<int, int>(ItemID1, Amount1);
                yield return new KeyValuePair<int, int>(ItemID2, Amount2);
                yield return new KeyValuePair<int, int>(ItemID3, Amount3);
            }
        }
        #endregion

        public Inventory(ulong id)
        {
            UserID = id;
            ItemID1 = ItemID2 = ItemID3 = Amount1 = Amount2 = Amount3 = 0;
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
                default:
                    return 0;
            }
        }

        public static void NewRecord(Inventory inventory)
        {
            SQL.Singleton.Connection.Execute("INSERT INTO Inventory (UserID, ItemID1, Amount1, ItemID2, Amount2, ItemID3, Amount3)" +
                " VALUES (@UserID ,@ItemID1, @Amount1, @ItemID2, @Amount2, @ItemID3, @Amount3)", inventory);
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
            }
        }

        public static Inventory Get(ulong ulUserID)
        {
            using (WriteSuspender wSus = new WriteSuspender())
            using (ReadSuspender rSus = new ReadSuspender())
                return SQL.Singleton.GetValue<Inventory>(m_sTableName, "*", m_sPrimaryKey, (long)ulUserID);
        }

        public static Item GetItemAtUserIndex(ulong ulUserID, int nIndex)
        {
            if (nIndex < 0 || nIndex > 2)
                return null;

            return SQL.Singleton.GetValue<Item>(m_sTableName, $"ItemID{nIndex}", m_sPrimaryKey, (long)ulUserID);
        }

        public void SetItemAtIndex(ulong ulUserID, int nIndex, int nItemID, int nAmount)
        {
            if (nIndex < 0 || nIndex > 2)
                return;

            Item item = Item.GetItemByID(nItemID);
            if (item == null)
                return;

            if (item.MaxStack < nAmount)
                return;

            Inventory inventory = Get(ulUserID);
            if (inventory == null)
                return;

            switch (nIndex)
            {
                case 1:
                    inventory.ItemID1 = nItemID;
                    inventory.Amount1 = nAmount;
                    break;
                case 2:
                    inventory.ItemID2 = nItemID;
                    inventory.Amount2 = nAmount;
                    break;
                case 3:
                    inventory.ItemID3 = nItemID;
                    inventory.Amount3 = nAmount;
                    break;
            }
        }

        protected override string OnGetTableName() => m_sTableName;
        protected override string OnGetPrimaryKeyName() => m_sPrimaryKey;
        protected override object OnGetPrimaryKeyValue() => SQLUser_ID;
    }
}
