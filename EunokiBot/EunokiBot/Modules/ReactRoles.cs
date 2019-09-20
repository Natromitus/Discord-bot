using System.Threading.Tasks;
using System.IO;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;
using System.Collections.Generic;

namespace EunokiBot.Modules
{
    [Group("roles"), Alias("role"), Summary("Roles commands.")]
    public class ReactRoles : ModuleBase<SocketCommandContext>
    {
        [Command("initialize"), Alias("init"), Summary("Create hardcoded role decision messages.")]
        public async Task ReactRolesInitializeAsync()
        { 
            if (!(Program.Singleton.Client.GetChannel(621645734651101194) is SocketTextChannel channel))
                return;

            List<GuildEmote> arGenderGEmotes = ReactManager.Singleton.GuildEmotesGender;
            RestUserMessage msgGender = await channel.SendMessageAsync(
                "**Gender**\n" +
                $"{arGenderGEmotes[0].ToString()} Male\n" +
                $"{arGenderGEmotes[1].ToString()} Female\n" +
                $"{arGenderGEmotes[2].ToString()} Other");

            List<IEmote> arGenderEmotes = ReactManager.Singleton.EmotesGender;
            await msgGender.AddReactionsAsync(new IEmote[] { arGenderEmotes[0], arGenderEmotes[1], arGenderEmotes[2] });

            List<GuildEmote> arHomeGEmotes = ReactManager.Singleton.GuildEmotesHome;
            RestUserMessage msgHome = await channel.SendMessageAsync("\n" +
                "**Homeland**\n" +
                $"{arHomeGEmotes[0].ToString()} North America\n" +
                $"{arHomeGEmotes[1].ToString()} South America\n" +
                $"{arHomeGEmotes[2].ToString()} Oceania\n" +
                $"{arHomeGEmotes[3].ToString()} Europe\n" +
                $"{arHomeGEmotes[4].ToString()} Asia\n" +
                $"{arHomeGEmotes[5].ToString()} Africa");

            List<IEmote> arHomeEmotes = ReactManager.Singleton.EmotesHome;
            await msgHome.AddReactionsAsync(new IEmote[]
            {
                arHomeEmotes[0], arHomeEmotes[1], arHomeEmotes[2], arHomeEmotes[3], arHomeEmotes[4], arHomeEmotes[5]
            });

            RestUserMessage msgZodiac = await channel.SendMessageAsync("\n" +
                "**Zodiac Sign**\n" +
                "♒ Aquarius\n" +
                "♓ Pisces\n" +
                "♈ Aries\n" +
                "♉ Taurus\n" +
                "♊ Gemini\n" +
                "♋ Cancer\n" +
                "♌ Leo\n" +
                "♍ Virgo\n" +
                "♎ Libra\n" +
                "♏ Scorpio\n" +
                "♐ Sagittarius\n" +
                "♑ Capricorn");

            List<Emoji> arEmojiZodiac = ReactManager.Singleton.EmojiZodiac;
            await msgZodiac.AddReactionsAsync(new IEmote[]
            {
                arEmojiZodiac[0], arEmojiZodiac[1], arEmojiZodiac[2],
                arEmojiZodiac[3], arEmojiZodiac[4], arEmojiZodiac[5],
                arEmojiZodiac[6], arEmojiZodiac[7], arEmojiZodiac[8],
                arEmojiZodiac[9], arEmojiZodiac[10], arEmojiZodiac[11]
            });

            List<GuildEmote> arMBTIGEmotes = ReactManager.Singleton.GuildEmotesMBTI;
            RestUserMessage msgMBTI = await channel.SendMessageAsync("\n" +
                "**MBTI Personality type**\n" +
                $"{arMBTIGEmotes[0].ToString()} INTJ - Architect\n" +
                $"{arMBTIGEmotes[1].ToString()} INTP - Logician\n" +
                $"{arMBTIGEmotes[2].ToString()} ENTJ - Commander\n" +
                $"{arMBTIGEmotes[3].ToString()} ENTP - Debater\n" +
                $"{arMBTIGEmotes[4].ToString()} INFJ - Advocate\n" +
                $"{arMBTIGEmotes[5].ToString()} INFP - Mediator\n" +
                $"{arMBTIGEmotes[6].ToString()} ENFJ - Protagonist\n" +
                $"{arMBTIGEmotes[7].ToString()} ENFP - Campaigner\n" +
                $"{arMBTIGEmotes[8].ToString()} ISTJ - Logistician\n" +
                $"{arMBTIGEmotes[9].ToString()} ISFJ - Defender\n" +
                $"{arMBTIGEmotes[10].ToString()} ESTJ - Executive\n" +
                $"{arMBTIGEmotes[11].ToString()} ESFJ - Consul\n" +
                $"{arMBTIGEmotes[12].ToString()} ISTP - Virtuoso\n" +
                $"{arMBTIGEmotes[13].ToString()} ISFP - Adventurer\n" +
                $"{arMBTIGEmotes[14].ToString()} ESTP - Entrerpreneur\n" +
                $"{arMBTIGEmotes[15].ToString()} ESFP - Entertainer");

            List<IEmote> arMBTIEmotes = ReactManager.Singleton.EmotesMBTI;
            await msgMBTI.AddReactionsAsync(new IEmote[]
            {
                arMBTIEmotes[0], arMBTIEmotes[1], arMBTIEmotes[2], arMBTIEmotes[3],
                arMBTIEmotes[4], arMBTIEmotes[5], arMBTIEmotes[6], arMBTIEmotes[7],
                arMBTIEmotes[8], arMBTIEmotes[9], arMBTIEmotes[10], arMBTIEmotes[11],
                arMBTIEmotes[12], arMBTIEmotes[13], arMBTIEmotes[14], arMBTIEmotes[15]
            });
        }
    }
}
