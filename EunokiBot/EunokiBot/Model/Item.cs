namespace EunokiBot.Model
{
    public class Item : Root
    {
        #region Fields
        private const string m_sTableName = "Items";
        private const string m_sPrimaryKey = "ItemID";
        #endregion

        #region Properties
        public int ItemID { get; private set; }
        public int Tier { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }
        public int MaxStack { get; private set; }
        #endregion

        protected Item()
        {
        }

        public static Item GetItemByID(int nID)
        {
            return SQL.Singleton.GetValue<Item>(m_sTableName, "*", m_sPrimaryKey, nID);
        }

        protected override string OnGetTableName() => m_sTableName;

        protected override string OnGetPrimaryKeyName() => m_sPrimaryKey;

        protected override object OnGetPrimaryKeyValue() => ItemID;
    }
}
