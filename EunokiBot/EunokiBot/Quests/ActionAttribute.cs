using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot.Quests
{
    public class ActionAttribute : Attribute    {
        public string Action { get; set; }

        public ActionAttribute(string action)
        {
            Action = action;
        }
    }
}
