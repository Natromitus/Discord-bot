using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using EunokiBot.Model;
using System.Reflection;

/*
- ImageEndings as array and property using it with IEnumberable<string>
- OnAction - user.CurrentQuests.ElementAt(i)
- Quest[] arQuests = user.CurrentQuests.Select(obj => Data.Singleton.Quests.FirstOrDefault(obj2 => obj2.QuestID == obj.Key)).ToArray();
- QuestTypes return only items with custom attribute != null
- BaseQuest.QuestInfo property and during the instantiate set to it the Quest (info)
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
                        obj => typeof(BaseQuest).IsAssignableFrom(obj)).ToList();
                }

                return m_arQuestTypes;
            }
        }
        #endregion

        public void OnAction(User user, ActionParam action)
        {
            Quest[] arQuests = user.CurrentQuests.Select(obj => Data.Singleton.Quests.FirstOrDefault(obj2 => obj2.QuestID == obj.Key)).ToArray();
            for (int i = 0; i < arQuests.Length; ++i)
                arQuests[i] = Data.Singleton.Quests.FirstOrDefault(obj => obj.QuestID == user.CurrentQuests.ToArray()[0].Key);

            List<BaseQuest> arBaseQuests = new List<BaseQuest>();

            foreach (Quest iter in arQuests)
            {
                TypeInfo foundClass = QuestTypes.FirstOrDefault(obj => obj.GetCustomAttribute<ActionAttribute>().Action == iter.Action);
                BaseQuest baseQuest = (BaseQuest)Activator.CreateInstance(foundClass);
                baseQuest.QuestInfo = iter;
                arBaseQuests.Add(baseQuest);
            }

            foreach (TypeInfo iter in QuestTypes)
            {
                ActionAttribute attr = iter.GetCustomAttribute<ActionAttribute>();
                if (attr.Action == action.Action)
                {
                    BaseQuest baseQuest = (BaseQuest)Activator.CreateInstance(iter);
                    baseQuest.
                    arBaseQuests.Add(baseQuest);
                    break;
                }
            }

            if (quest == null)
                return;

            // TODO GET QUEST HERE HOW?
            quest.OnActionProcess(user, , action.Parameter);
        }
    }
}