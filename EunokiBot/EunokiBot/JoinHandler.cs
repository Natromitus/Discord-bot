using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord.WebSocket;

namespace EunokiBot
{
    class JoinHandler
    {
        public async Task OnUserJoin(SocketGuildUser user)
        {
            Data.Data.SaveUser(new UserModel(user.Id));
        }
    }
}
