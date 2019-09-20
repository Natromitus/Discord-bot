using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

using EunokiBot.Model;

namespace EunokiBot.Quests
{
    public abstract class BaseQuest
    {
        public Quest QuestInfo { get; set; }

        public void Action(User user, ulong param, bool bRemove)
        {
            if (OnActionProcess(QuestInfo, param))
                Progress(user, QuestInfo, bRemove);


        }

        public abstract bool OnActionProcess(Quest quest, ulong param);

        private void Progress(User user, Quest quest, bool bRemove)
        {
            List<int> arCurrentQuestsID = user.CurrentQuests.ToArray().Select(
                obj => obj.Key).ToList();
            List<int> arSelectedQuestsID = arCurrentQuestsID.Where(
                obj => obj == quest.QuestID).ToList();

            List<int> arSelectedIndex = new List<int>();
            int nStartPos = 0;
            foreach(int iter in arSelectedQuestsID)
            {
                int nFound = arCurrentQuestsID.IndexOf(iter, nStartPos);
                arSelectedIndex.Add(nFound);
                nStartPos = nFound + 1;
            }

            if(bRemove)
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
