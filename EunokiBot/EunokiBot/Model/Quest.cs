namespace EunokiBot.Model
{
    public class Quest : Root
    {
        #region Fields
        public const string TABLE_NAME = "Quests";
        public const string PRIMARY_KEY = "QuestID";
        #endregion

        #region Properties
        public int QuestID { get; private set; }
        public int Difficulty { get; private set; }
        public int Action { get; private set; }
        public int Amount { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Specification { get; private set; }
        #endregion

        protected Quest()
        {
        }

        protected override string OnGetTableName() => TABLE_NAME;

        protected override string OnGetPrimaryKeyName() => PRIMARY_KEY;

        protected override object OnGetPrimaryKeyValue() => QuestID;
    }
}
