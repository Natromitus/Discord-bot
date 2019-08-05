using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot.Model
{
    public class Level : Root
    {
        #region Fields
        private const string m_sTableName = "Levels";
        private const string m_sPrimaryKey = "Level";
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
            return SQL.Singleton.GetValue<Level>(m_sTableName, "*", m_sPrimaryKey, nLevelIndex);
        }

        protected override string OnGetTableName() => m_sTableName;

        protected override string OnGetPrimaryKeyName() => m_sPrimaryKey;

        protected override object OnGetPrimaryKeyValue() => LevelIndex;
    }
}
