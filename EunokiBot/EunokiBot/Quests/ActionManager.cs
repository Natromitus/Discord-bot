using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using EunokiBot.Model;
using System.Reflection;

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
            Quest[] arQuests = new Quest[3];

            for(int i = 0; i < arQuests.Length; ++i)
                arQuests[i] = Data.Singleton.Quests.FirstOrDefault(obj => obj.QuestID == user.CurrentQuests.ToArray()[0].Key);

            BaseQuest quest = null;
            foreach (TypeInfo iter in QuestTypes)
            {
                ActionAttribute attr = iter.GetCustomAttribute<ActionAttribute>();
                if (attr == null)
                    continue;

                if (attr.Action == action.Action)
                {
                    quest = (BaseQuest)Activator.CreateInstance(iter);
                    break;
                }
            }

            if (quest == null)
                return;

            // TODO GET QUEST HERE HOW? I DUNNO BITCH JUST DO IT
            quest.OnActionProcess(user, , action.Parameter);
        }
    }
}