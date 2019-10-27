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
        #endregion

        public void OnAction(User user, ActionParam action, bool bRemove = false)
        {
            // Get User's Quests
            Quest[] arUserQuests = user.CurrentQuests.Select(obj => Data.Singleton.Quests.FirstOrDefault(
                obj2 => obj2.QuestID == obj.Key)).ToArray();

            // Find quests that have same specification first or without specification
            List<Quest> quests = arUserQuests.Where(obj => obj.Action == action.Action).Where(
                obj => obj.Specification == action.Parameter || obj.Specification == 0).ToList();

            if (quests.Count == 0)
                return;

            foreach(Quest quest in quests)
                Progress(user, quest, bRemove);
        }

        private void Progress(User user, Quest quest, bool bRemove)
        {
            List<int> arCurrentQuestsID = user.CurrentQuests.ToArray().Select(
                obj => obj.Key).ToList();
            List<int> arSelectedQuestsID = arCurrentQuestsID.Where(
                obj => obj == quest.QuestID).ToList();

            List<int> arSelectedIndex = new List<int>();
            int nStartPos = 0;
            foreach (int iter in arSelectedQuestsID)
            {
                int nFound = arCurrentQuestsID.IndexOf(iter, nStartPos);
                arSelectedIndex.Add(nFound);
                nStartPos = nFound + 1;
            }

            if (bRemove)
            {
                foreach (int iter in arSelectedIndex)
                    user.RemoveProgressOnIndex(iter);

                return;
            }

            foreach (int iter in arSelectedIndex)
            {
                if (user.AddProgressOnIndex(iter, quest.Amount))
                {
                    QuestReward reward = QuestReward.GetRewardByDifficulty(quest.Difficulty);
                    user.XP += reward.XP;
                    ++user.Quests;

                    float fRnd1 = (float)new Random().NextDouble();
                    if (fRnd1 < reward.ChanceGold)
                        user.Money += reward.Gold;

                    float fRnd2 = (float)new Random().NextDouble();
                    if (fRnd2 < reward.MBoxChance)
                    {
                        Inventory inventory = new Inventory(user.UserID);
                        inventory.AddItem(17, 1);
                    }
                }
            }
        }
    }
}