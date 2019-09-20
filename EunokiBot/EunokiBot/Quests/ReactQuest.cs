using System;
using System.Collections.Generic;
using System.Text;
using EunokiBot.Model;

namespace EunokiBot.Quests
{
    [ActionAttribute("React")]
    public class ReactQuest : BaseQuest
    {
        public override bool OnActionProcess(Quest quest, ulong param)
        {
            if (quest.Specification == 0 || quest.Specification == param)
                return true;

            return false;
        }
    }
}
