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

        public void Action(User user, ulong param)
        {
            if (OnActionProcess(user, QuestInfo, param))
                AddProgress(user, QuestInfo);
        }

        public abstract bool OnActionProcess(User user, Quest quest, ulong param);

        private void AddProgress(User user, Quest quest)
        {
            int userQuestIndex = user.CurrentQuests.ToList().FindIndex(obj => obj.Key == quest.QuestID);
            if(user.AddProgressOnIndex(userQuestIndex, quest.Amount))
            {
                QuestReward reward = QuestReward.GetRewardByDifficulty(quest.Difficulty);
                user.XP += reward.XP;
                
                

            }
        }
    }
}
