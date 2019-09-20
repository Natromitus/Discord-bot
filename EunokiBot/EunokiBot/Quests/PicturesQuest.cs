using System;
using System.Collections.Generic;
using System.Text;
using EunokiBot.Model;

namespace EunokiBot.Quests
{
    [ActionAttribute("Pictures")]
    public class PicturesQuest : BaseQuest
    {
        public override bool OnActionProcess(Quest quest, ulong param)
        {
            if (quest.Specification == 0 || quest.Specification == param)
                return true;

            return false;
        }
    }
}
