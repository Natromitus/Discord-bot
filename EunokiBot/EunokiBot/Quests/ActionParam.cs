using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot.Quests
{
    public class ActionParam
    {
        public string Action { get; private set; }
        public ulong Parameter { get; private set; }

        public ActionParam(string sAction, ulong nParam)
        {
            Action = sAction;
            Parameter = nParam;
        }
    }
}
