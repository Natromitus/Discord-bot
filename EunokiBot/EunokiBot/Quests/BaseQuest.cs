using System;
using System.Collections.Generic;
using System.Text;

using EunokiBot.Model;

namespace EunokiBot.Quests
{
    public abstract class BaseQuest
    {
        public Quest QuestInfo { get; set; }

        public void OnAction(User user, ulong param)
        {
            if (OnActionProcess(user, QuestInfo, param))
                AddProgress(user, QuestInfo);
        }

        public abstract bool OnActionProcess(User user, Quest quest, ulong param);

        public abstract bool AddProgress(User user, Quest quest);
    }
}
