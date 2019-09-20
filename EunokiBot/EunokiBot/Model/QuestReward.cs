using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot.Model
{
    public class QuestReward : Root
    {
        #region Fields
        public const string TABLE_NAME = "QuestsAttributes";
        public const string PRIMARY_KEY = "Difficulty";
        #endregion

        #region Properties
        public int Difficulty { get; private set; }
        public int XP { get; private set; }
        public float ChanceGold { get; private set; }
        public int Gold { get; private set; }
        public float MBoxChance { get; private set; }

        protected QuestReward()
        {
        }

        public static QuestReward GetRewardByDifficulty(int nDifficulty)
        {
            return SQL.Singleton.GetValue<QuestReward>(TABLE_NAME, "*", PRIMARY_KEY, nDifficulty);
        }

        protected override string OnGetTableName() => TABLE_NAME;

        protected override string OnGetPrimaryKeyName() => PRIMARY_KEY;

        protected override object OnGetPrimaryKeyValue() => Difficulty;
        #endregion
    }
}
