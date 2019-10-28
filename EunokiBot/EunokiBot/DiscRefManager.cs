using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot
{
    public class DiscRefManager
    {
        private static readonly DiscRefManager m_singleton = new DiscRefManager();

        private SocketGuild m_guild = null;
        private SocketTextChannel m_channelMain = null;
        private SocketTextChannel m_channelSuggestions = null;
        private SocketTextChannel m_channelForFun = null;
        private SocketTextChannel m_channelInfo = null; 

        public static DiscRefManager Singleton => m_singleton;

        public SocketGuild Guild
        {
            get
            {
                if (m_guild == null)
                    m_guild = Program.Singleton.Client.GetGuild(Convert.ToUInt64(Config.Bot.guild));

                return m_guild;
            }
        }

        public SocketTextChannel ChannelMain
        {
            get
            {
                if (m_channelMain == null)
                    m_channelMain = Program.Singleton.Client.GetChannel(Convert.ToUInt64(Config.Bot.channelMain)) as SocketTextChannel;

                return m_channelMain;
            }
        }

        public SocketTextChannel ChannelSuggestions
        {
            get
            {
                if (m_channelSuggestions == null)
                    m_channelSuggestions = Program.Singleton.Client.GetChannel(Convert.ToUInt64(Config.Bot.channelSuggestions)) as SocketTextChannel;

                return m_channelSuggestions;
            }
        }

        public SocketTextChannel ChannelForFun
        {
            get
            {
                if (m_channelForFun == null)
                    m_channelForFun = Program.Singleton.Client.GetChannel(Convert.ToUInt64(Config.Bot.channelForFun)) as SocketTextChannel;

                return m_channelForFun;
            }
        }

        public SocketTextChannel ChannelInfo
        {
            get
            {
                if (m_channelInfo == null)
                    m_channelInfo = Program.Singleton.Client.GetChannel(Convert.ToUInt64(Config.Bot.channelInfo)) as SocketTextChannel;

                return m_channelInfo;
            }
        }

    }
}
