using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using EunokiBot.Model;
using System.Reflection;

/*
- ImageEndings as array and property using it with IEnumberable<string>
*/

namespace EunokiBot.Quests
{
    public class ActionManager
    {
        #region Fields
        private static readonly ActionManager m_singleton = new ActionManager();

        private List<TypeInfo> m_arQuestTypes = null;
        #endregion

        #region Properties
        public static ActionManager Singleton => m_singleton;

        private IEnumerable<TypeInfo> QuestTypes
        {
            get
            {
                if (m_arQuestTypes == null)
                {
                    Assembly assy = Assembly.GetEntryAssembly();
                    m_arQuestTypes = assy.DefinedTypes.Where(
                        obj => typeof(BaseQuest).IsAssignableFrom(obj) && obj.GetCustomAttribute<ActionAttribute>() != null).ToList();
                }

                return m_arQuestTypes;
            }
        }
        #endregion

        public void OnAction(User user, ActionParam action)
        {
            Quest[] arQuests = user.CurrentQuests.Select(obj => Data.Singleton.Quests.FirstOrDefault(obj2 => obj2.QuestID == obj.Key)).ToArray();
            List<BaseQuest> arBaseQuests = new List<BaseQuest>();

            foreach (Quest iter in arQuests)
            {
                TypeInfo foundClass = QuestTypes.FirstOrDefault(obj => obj.GetCustomAttribute<ActionAttribute>().Action == iter.Action);
                BaseQuest baseQuest = (BaseQuest)Activator.CreateInstance(foundClass);
                baseQuest.QuestInfo = iter;
                arBaseQuests.Add(baseQuest);
            }

            foreach(BaseQuest baseQuest in arBaseQuests)
                baseQuest.OnAction(user, action.Parameter);
        }
    }
}