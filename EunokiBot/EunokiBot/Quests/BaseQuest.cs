using System;
using System.Collections.Generic;
using System.Text;

using EunokiBot.Model;

namespace EunokiBot.Quests
{
    public abstract class BaseQuest
    {
        public abstract void OnActionProcess(User user, Quest quest, int param);
    }
}
