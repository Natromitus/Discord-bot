using EunokiBot.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EunokiBot.Model
{
    public class Data
    {
        #region Fields
        private static readonly Data m_singleton = new Data();

        private int m_nShopPages = 0;
        private List<TypeInfo> m_arItemTypes = null;
        private List<Level> m_arLevels = null;
        private List<Item> m_arItems = null;
        private List<Quest> m_arQuests = null;
        private List<Quest> m_arEasyQuests = null;
        private List<Quest> m_arMediumQuests = null;
        private List<Quest> m_arHardQuests = null;
        private List<Quest> m_arLegendaryQuests = null;

        private List<string> m_arImageEndings = null;
        #endregion

        #region Properties
        public static Data Singleton => m_singleton;

        public int ShopPages
        {
            get
            {
                if(m_nShopPages == 0)
                {
                    int nCount = SQL.Singleton.GetCount(Item.TABLE_NAME);

                    int nPages = nCount / 12;
                    if (nCount % 12 != 0)
                        ++nPages;

                    m_nShopPages = nPages;
                }

                return m_nShopPages;
            }
        }

        public List<Level> Levels
        {
            get
            {
                if(m_arLevels == null)
                {
                    m_arLevels = new List<Level>();

                    int nCount = SQL.Singleton.GetCount(Level.TABLE_NAME);
                    for (int i = 0; i <= nCount; ++i)
                        m_arLevels.Add(SQL.Singleton.GetValue<Level>(Level.TABLE_NAME, "*", Level.PRIMARY_KEY, i));
                }

                return m_arLevels;
            }
        }

        public List<Item> Items
        {
            get
            {
                if (m_arItems == null)
                {
                    m_arItems = new List<Item>();

                    int nCount = SQL.Singleton.GetCount(Item.TABLE_NAME);
                    for (int i = 1; i <= nCount; ++i)
                        m_arItems.Add(SQL.Singleton.GetValue<Item>(Item.TABLE_NAME, "*", Item.PRIMARY_KEY, i));
                }

                return m_arItems;
            }
        }

        public IEnumerable<TypeInfo> ItemTypes
        {
            get
            {
                if (m_arItemTypes == null)
                {
                    Assembly assy = Assembly.GetEntryAssembly();
                    m_arItemTypes = assy.DefinedTypes.Where(
                        obj => typeof(BaseItem).IsAssignableFrom(obj)).ToList();
                }

                return m_arItemTypes;
            }
        }

        public List<Quest> Quests
        {
            get
            {
                if(m_arQuests == null)
                {
                    m_arQuests = new List<Quest>();

                    int nCount = SQL.Singleton.GetCount(Quest.TABLE_NAME);
                    for (int i = 1; i <= nCount; ++i)
                        m_arQuests.Add(SQL.Singleton.GetValue<Quest>(Quest.TABLE_NAME, "*", Quest.PRIMARY_KEY, i));
                }

                return m_arQuests;
            }
        }

        public List<Quest> EasyQuests
        {
            get
            {
                if(m_arEasyQuests == null)
                    m_arEasyQuests = Quests.Where(obj => obj.Difficulty == 1).ToList();

                return m_arEasyQuests;
            }
        }

        public List<Quest> MediumQuests
        {
            get
            {
                if (m_arMediumQuests == null)
                    m_arMediumQuests = Quests.Where(obj => obj.Difficulty == 2).ToList();

                return m_arMediumQuests;
            }
        }

        public List<Quest> HardQuests
        {
            get
            {
                if (m_arHardQuests == null)
                    m_arHardQuests = Quests.Where(obj => obj.Difficulty == 3).ToList();

                return m_arHardQuests;
            }
        }

        public List<Quest> LegendaryQuests
        {
            get
            {
                if (m_arLegendaryQuests == null)
                    m_arLegendaryQuests = Quests.Where(obj => obj.Difficulty == 4).ToList();

                return m_arLegendaryQuests;
            }
        }

        public List<string> ImageEndings
        {
            get
            {
                if(m_arImageEndings == null)
                {
                    m_arImageEndings.Add(".png");
                    m_arImageEndings.Add(".jpg");
                    m_arImageEndings.Add(".jpeg");
                    m_arImageEndings.Add(".tiff");
                    m_arImageEndings.Add(".gif");
                    m_arImageEndings.Add(".bmp");
                }

                return m_arImageEndings;
            }
        }

        #endregion
    }
}
