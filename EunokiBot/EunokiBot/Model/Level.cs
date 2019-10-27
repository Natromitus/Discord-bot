using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot.Model
{
    public class Level : Root
    {
        #region Fields
        public const string TABLE_NAME = "Levels";
        public const string PRIMARY_KEY = "Level";
        #endregion

        #region Properties
        public int LevelIndex { get; private set; }
        public string Name { get; private set; }
        public int XPGap { get; private set; }
        public int XPPerMessage { get; private set; }
        public float ChanceEasyQ { get; private set; }
        public float ChanceMediumQ { get; private set; }
        public float ChanceHardQ { get; private set; }
        public float ChanceLegendaryQ { get; private set; }
        #endregion

        protected Level()
        {
        }

        public static Level GetLevel(int nLevelIndex)
        {
            return SQL.Singleton.GetValue<Level>(TABLE_NAME, "*", PRIMARY_KEY, nLevelIndex);
        }

        protected override string OnGetTableName() => TABLE_NAME;

        protected override string OnGetPrimaryKeyName() => PRIMARY_KEY;

        protected override object OnGetPrimaryKeyValue() => LevelIndex;
    }
}