using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot.Model
{
    public class Data
    {
        #region Fields
        private static readonly Data m_singleton = new Data();

        private List<Level> m_arLevels = new List<Level>();
        private List<Item> m_arItems = new List<Item>();
        //private List<Action> m_arActions = new List<Action>();
        //private List<Quest> m_arQuests = new List<Quest>();
        #endregion

        #region Properties
        public static Data Singleton => m_singleton;

        public List<Level> Levels
        {
            get
            {
                if(m_arLevels.Count == 0)
                {
                    int nCount = SQL.Singleton.GetCount(Level.TABLE_NAME);
                    for (int i = 0; i < nCount; ++i)
                        m_arLevels.Add(SQL.Singleton.GetValue<Level>(Level.TABLE_NAME, "*", Level.PRIMARY_KEY, i));
                }

                return m_arLevels;
            }
        }

        public List<Item> Items
        {
            get
            {
                if (m_arLevels.Count == 0)
                {
                    int nCount = SQL.Singleton.GetCount(Item.TABLE_NAME);
                    for (int i = 0; i < nCount; ++i)
                        m_arItems.Add(SQL.Singleton.GetValue<Item>(Item.TABLE_NAME, "*", Item.PRIMARY_KEY, i));
                }

                return m_arItems;
            }
        }
        #endregion
    }
}
