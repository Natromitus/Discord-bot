namespace EunokiBot.Model
{
    public class Item : Root
    {
        #region Fields
        public const string TABLE_NAME = "Items";
        public const string PRIMARY_KEY = "ItemID";
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
            return SQL.Singleton.GetValue<Item>(TABLE_NAME, "*", PRIMARY_KEY, nID);
        }

        protected override string OnGetTableName() => TABLE_NAME;

        protected override string OnGetPrimaryKeyName() => PRIMARY_KEY;

        protected override object OnGetPrimaryKeyValue() => ItemID;
    }
}
