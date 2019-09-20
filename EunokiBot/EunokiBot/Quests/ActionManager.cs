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
                        obj => typeof(BaseQuest).IsAssignableFrom(obj) && obj.GetCustomAttribute<ActionAttribute>() != null).ToList();
                }

                return m_arQuestTypes;
            }
        }
        #endregion

        public void OnAction(User user, ActionParam action, bool bRemove = false)
        {
            Quest[] arUserQuests = user.CurrentQuests.Select(
                obj => Data.Singleton.Quests.FirstOrDefault(
                obj2 => obj2.QuestID == obj.Key)).ToArray();

            Quest quest = arUserQuests.FirstOrDefault(obj => obj.Action == action.Action);
            if (quest == null)
                return;

            TypeInfo foundClass = QuestTypes.FirstOrDefault(
                obj => obj.GetCustomAttribute<ActionAttribute>().Action == quest.Action);
            BaseQuest baseQuest = (BaseQuest)Activator.CreateInstance(foundClass);
            baseQuest.QuestInfo = quest;

            baseQuest.Action(user, action.Parameter, bRemove);
        }
    }
}