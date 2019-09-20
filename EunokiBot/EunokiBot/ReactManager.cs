using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using EunokiBot.Model;
using EunokiBot.Quests;

namespace EunokiBot
{
    public class ReactManager
    {
        #region Fields
        private static readonly ReactManager m_singleton = new ReactManager();

        private List<GuildEmote> m_arGEmotesHome = null;
        private List<GuildEmote> m_arGEmotesGender = null;
        private List<Emoji> m_arEmojiZodiac = null;
        private List<GuildEmote> m_arGEmotesMBTI = null;

        private List<IEmote> m_arEmotesHome = null;
        private List<IEmote> m_arEmotesGender = null;
        private List<IEmote> m_arEmoteZodiac = null;
        private List<IEmote> m_arEmoteMBTI = null;

        private List<IRole> m_arGenderRoles = null;
        private List<IRole> m_arHomeRoles = null;
        private List<IRole> m_arZodiacRoles = null;
        private List<IRole> m_arMBTIRoles = null;

        #endregion

        #region Properties
        public static ReactManager Singleton => m_singleton;

        #region HomeReact
        public List<GuildEmote> GuildEmotesHome
        {
            get
            {
                if(m_arGEmotesHome == null)
                {
                    m_arGEmotesHome = new List<GuildEmote>();

                    SocketGuild guild = Program.Singleton.Client.GetGuild(573131665660968970);

                    m_arGEmotesHome.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "northamerica"));
                    m_arGEmotesHome.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "southamerica"));
                    m_arGEmotesHome.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "oceania"));
                    m_arGEmotesHome.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "europe"));
                    m_arGEmotesHome.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "asia"));
                    m_arGEmotesHome.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "africa"));
                }

                return m_arGEmotesHome;
            }
        }
        public List<IEmote> EmotesHome
        {
            get
            {
                if (m_arEmotesHome == null)
                {
                    m_arEmotesHome = new List<IEmote>();

                    m_arEmotesHome = GuildEmotesHome.Select(obj => (IEmote)obj).ToList();
                }

                return m_arEmotesHome;
            }
        }
        private IEnumerable<IRole> RolesHome
        {
            get
            {
                if (m_arHomeRoles == null)
                {
                    m_arHomeRoles = new List<IRole>();

                    SocketGuild guild = Program.Singleton.Client.GetGuild(573131665660968970);

                    m_arHomeRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "North America"));
                    m_arHomeRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "South America"));
                    m_arHomeRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Oceania"));
                    m_arHomeRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Europe"));
                    m_arHomeRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Asia"));
                    m_arHomeRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Africa"));
                }

                return m_arHomeRoles;
            }
        }
        #endregion

        #region GenderReact
        public List<GuildEmote> GuildEmotesGender
        {
            get
            {
                if(m_arGEmotesGender == null)
                {
                    m_arGEmotesGender = new List<GuildEmote>();

                    SocketGuild guild = Program.Singleton.Client.GetGuild(573131665660968970);

                    m_arGEmotesGender.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "male"));
                    m_arGEmotesGender.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "female"));
                    m_arGEmotesGender.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "other"));
                }

                return m_arGEmotesGender;
            }
        }
        public List<IEmote> EmotesGender
        {
            get
            {
                if (m_arEmotesGender == null)
                {
                    m_arEmotesGender = new List<IEmote>();

                    m_arEmotesGender = GuildEmotesGender.Select(obj => (IEmote)obj).ToList();
                }

                return m_arEmotesGender;
            }
        }
        private IEnumerable<IRole> RolesGender
        {
            get
            {
                if(m_arGenderRoles == null)
                {
                    m_arGenderRoles = new List<IRole>();

                    SocketGuild guild = Program.Singleton.Client.GetGuild(573131665660968970);

                    m_arGenderRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Male"));
                    m_arGenderRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Female"));
                    m_arGenderRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Other"));
                }

                return m_arGenderRoles;
            }
        }
        #endregion

        #region ZodiacReact
        public List<Emoji> EmojiZodiac
        {
            get
            {
                if(m_arEmojiZodiac == null)
                {
                    m_arEmojiZodiac = new List<Emoji>();

                    m_arEmojiZodiac.Add(new Emoji("♒"));
                    m_arEmojiZodiac.Add(new Emoji("♓"));
                    m_arEmojiZodiac.Add(new Emoji("♈"));
                    m_arEmojiZodiac.Add(new Emoji("♉"));
                    m_arEmojiZodiac.Add(new Emoji("♊"));
                    m_arEmojiZodiac.Add(new Emoji("♋"));
                    m_arEmojiZodiac.Add(new Emoji("♌"));
                    m_arEmojiZodiac.Add(new Emoji("♍"));
                    m_arEmojiZodiac.Add(new Emoji("♎"));
                    m_arEmojiZodiac.Add(new Emoji("♏"));
                    m_arEmojiZodiac.Add(new Emoji("♐"));
                    m_arEmojiZodiac.Add(new Emoji("♑"));
                }

                return m_arEmojiZodiac;
            }
        }

        public List<IEmote> EmotesZodiac
        {
            get
            {
                if (m_arEmoteZodiac == null)
                {
                    m_arEmoteZodiac = new List<IEmote>();

                    m_arEmoteZodiac = EmojiZodiac.Select(obj => (IEmote)obj).ToList();
                }

                return m_arEmoteZodiac;
            }
        }

        private IEnumerable<IRole> RolesZodiac
        {
            get
            {
                if (m_arZodiacRoles == null)
                {
                    m_arZodiacRoles = new List<IRole>();

                    SocketGuild guild = Program.Singleton.Client.GetGuild(573131665660968970);

                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Aquarius"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Pisces"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Aries"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Taurus"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Gemini"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Cancer"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Leo"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Virgo"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Libra"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Scorpio"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Sagittarius"));
                    m_arZodiacRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "Capricorn"));
                }

                return m_arZodiacRoles;
            }
        }
        #endregion

        #region MBTIReact
        public List<GuildEmote> GuildEmotesMBTI
        {
            get
            {
                if(m_arGEmotesMBTI == null)
                {
                    m_arGEmotesMBTI = new List<GuildEmote>();

                    SocketGuild guild = Program.Singleton.Client.GetGuild(573131665660968970);

                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "intj"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "intp"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "entj"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "entp"));

                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "infj"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "infp"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "enfj"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "enfp"));

                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "istj"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "isfj"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "estj"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "esfj"));

                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "istp"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "isfp"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "estp"));
                    m_arGEmotesMBTI.Add(guild.Emotes.FirstOrDefault(obj => obj.Name == "esfp"));
                }

                return m_arGEmotesMBTI;
            }
        }
        public List<IEmote> EmotesMBTI
        {
            get
            {
                if (m_arEmoteMBTI == null)
                {
                    m_arEmoteMBTI = new List<IEmote>();

                    m_arEmoteMBTI = GuildEmotesMBTI.Select(obj => (IEmote)obj).ToList();
                }

                return m_arEmoteMBTI;
            }
        }

        private IEnumerable<IRole> RolesMBTI
        {
            get
            {
                if (m_arMBTIRoles == null)
                {
                    m_arMBTIRoles = new List<IRole>();

                    SocketGuild guild = Program.Singleton.Client.GetGuild(573131665660968970);

                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "INTJ"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "INTP"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ENTJ"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ENTP"));

                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "INFJ"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "INFP"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ENFJ"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ENFP"));

                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ISTJ"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ISFJ"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ESTJ"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ESFJ"));

                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ISTP"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ISFP"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ESTP"));
                    m_arMBTIRoles.Add(guild.Roles.FirstOrDefault(obj => obj.Name == "ESFP"));
                }

                return m_arMBTIRoles;
            }
        }
        #endregion
        #endregion

        public async Task ReactionAddedAsync(
            Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            SocketGuild guild = Program.Singleton.Client.GetGuild(573131665660968970);
            SocketGuildUser gUser = guild.GetUser(reaction.UserId);
            if (gUser == null)
                return;

            IUser iUser = reaction.User.Value;
            IUserMessage msg = await cache.DownloadAsync();

            if (reaction.MessageId.ToString() == Config.Bot.reactGender)
            {
                await msg.RemoveReactionsAsync(iUser, EmotesGender.Where(obj => obj.Name != reaction.Emote.Name).ToArray());
                await gUser.RemoveRolesAsync(RolesGender.Intersect(gUser.Roles.Select(obj => (IRole)obj)));

                switch (reaction.Emote.Name)
                {   
                    case "male":
                        _ = gUser.AddRoleAsync(RolesGender.FirstOrDefault(obj => obj.Name.ToString() == "Male"));
                        break;
                    case "female":
                        _ = gUser.AddRoleAsync(RolesGender.FirstOrDefault(obj => obj.Name.ToString() == "Female"));
                        break;
                    case "other":
                        _ = gUser.AddRoleAsync(RolesGender.FirstOrDefault(obj => obj.Name.ToString() == "Other"));
                        break;
                    default:
                        _ = msg.RemoveReactionAsync(reaction.Emote, iUser, RequestOptions.Default);
                        break;
                }
            }
            else if (reaction.MessageId.ToString() == Config.Bot.reactHome)
            {
                await msg.RemoveReactionsAsync(iUser, EmotesHome.Where(obj => obj.Name != reaction.Emote.Name).ToArray());
                await gUser.RemoveRolesAsync(RolesHome.Intersect(gUser.Roles.Select(obj => (IRole)obj)));


                switch (reaction.Emote.Name)
                {
                    case "northamerica":
                        _ = gUser.AddRoleAsync(RolesHome.FirstOrDefault(obj => obj.Name.ToString() == "North America"));
                        break;
                    case "southamerica":
                        _ = gUser.AddRoleAsync(RolesHome.FirstOrDefault(obj => obj.Name.ToString() == "South America"));
                        break;
                    case "oceania":
                        _ = gUser.AddRoleAsync(RolesHome.FirstOrDefault(obj => obj.Name.ToString() == "Oceania"));
                        break;
                    case "europe":
                        _ = gUser.AddRoleAsync(RolesHome.FirstOrDefault(obj => obj.Name.ToString() == "Europe"));
                        break;
                    case "asia":
                        _ = gUser.AddRoleAsync(RolesHome.FirstOrDefault(obj => obj.Name.ToString() == "Asia"));
                        break;
                    case "africa":
                        _ = gUser.AddRoleAsync(RolesHome.FirstOrDefault(obj => obj.Name.ToString() == "Africa"));
                        break;
                    default:
                        _ = msg.RemoveReactionAsync(reaction.Emote, iUser);
                        break;
                }
            }
            else if (reaction.MessageId.ToString() == Config.Bot.reactZodiac)
            {
                await msg.RemoveReactionsAsync(iUser, EmotesZodiac.Where(obj => obj.Name != reaction.Emote.Name).ToArray());
                await gUser.RemoveRolesAsync(RolesZodiac.Intersect(gUser.Roles.Select(obj => (IRole)obj)));

                switch (reaction.Emote.Name)
                {
                    case "♒":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Aquarius"));
                        break;
                    case "♓":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Pisces"));
                        break;
                    case "♈":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Aries"));
                        break;
                    case "♉":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Taurus"));
                        break;
                    case "♊":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Gemini"));
                        break;
                    case "♋":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Cancer"));
                        break;
                    case "♌":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Leo"));
                        break;
                    case "♍":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Virgo"));
                        break;
                    case "♎":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Libra"));
                        break;
                    case "♏":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Scorpio"));
                        break;
                    case "♐":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Sagittarius"));
                        break;
                    case "♑":
                        _ = gUser.AddRoleAsync(RolesZodiac.FirstOrDefault(obj => obj.Name.ToString() == "Capricorn"));
                        break;
                    default:
                        _ = msg.RemoveReactionAsync(reaction.Emote, iUser);
                        break;
                }
            }
            else if (reaction.MessageId.ToString() == Config.Bot.reactMBTI)
            {
                await msg.RemoveReactionsAsync(iUser, EmotesMBTI.Where(obj => obj.Name != reaction.Emote.Name).ToArray());
                await gUser.RemoveRolesAsync(RolesMBTI.Intersect(gUser.Roles.Select(obj => (IRole)obj)));

                switch (reaction.Emote.Name)
                {
                    case "intj":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "INTJ"));
                        break;
                    case "intp":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "INTP"));
                        break;
                    case "entj":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ENTJ"));
                        break;
                    case "entp":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ENTP"));
                        break;
                    case "infj":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "INFJ"));
                        break;
                    case "infp":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "INFP"));
                        break;
                    case "enfj":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ENFJ"));
                        break;
                    case "enfp":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ENFP"));
                        break;
                    case "istj":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ISTJ"));
                        break;
                    case "isfj":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ISFJ"));
                        break;
                    case "estj":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ESTJ"));
                        break;
                    case "esfj":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ESFJ"));
                        break;
                    case "istp":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ISTP"));
                        break;
                    case "isfp":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ISFP"));
                        break;
                    case "estp":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ESTP"));
                        break;
                    case "esfp":
                        _ = gUser.AddRoleAsync(RolesMBTI.FirstOrDefault(obj => obj.Name.ToString() == "ESFP"));
                        break;
                    default:
                        _ = msg.RemoveReactionAsync(reaction.Emote, iUser);
                        break;
                }
            }

            User user = User.Get(reaction.UserId);
            if (user == null)
                return;

            ActionParam action = new ActionParam("React", channel.Id);
            ActionManager.Singleton.OnAction(user, action);
        }

        public async Task ReactionRemovedAsync(
            Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            SocketGuildUser gUser = Program.Singleton.Client.GetGuild(573131665660968970).GetUser(reaction.UserId);

            if (reaction.MessageId.ToString() == Config.Bot.reactGender)
                _ = gUser.RemoveRolesAsync(RolesGender.Intersect(gUser.Roles.Select(obj => (IRole)obj)));
            else if (reaction.MessageId.ToString() == Config.Bot.reactHome)
                _ = gUser.RemoveRolesAsync(RolesHome.Intersect(gUser.Roles.Select(obj => (IRole)obj)));
            else if (reaction.MessageId.ToString() == Config.Bot.reactZodiac)
                _ = gUser.RemoveRolesAsync(RolesZodiac.Intersect(gUser.Roles.Select(obj => (IRole)obj)));
            else if (reaction.MessageId.ToString() == Config.Bot.reactMBTI)
                _ = gUser.RemoveRolesAsync(RolesMBTI.Intersect(gUser.Roles.Select(obj => (IRole)obj)));

            User user = User.Get(reaction.UserId);
            if (user == null)
                return;

            ActionParam action = new ActionParam("React", channel.Id);
            ActionManager.Singleton.OnAction(user, action, true);
        }
    }
}
