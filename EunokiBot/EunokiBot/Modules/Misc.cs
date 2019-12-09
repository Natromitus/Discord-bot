using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using EunokiBot.Model;
using EunokiBot.ImageManagment;

namespace EunokiBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("notifications toggle"), Alias("notif toggle", "notification toggle")]
        public async Task NotificationsToggleAsync()
        {
            User user = User.Get(Context.User.Id);

            if (user.Notifications == 0)
            {
                user.Notifications = 1;
                _ = Context.Channel.SendMessageAsync("Notifications turned on!");
            }
            else
            {
                user.Notifications = 0;
                _ = Context.Channel.SendMessageAsync("Notifications turned off!");
            }
        }

        [Command("patreon refresh"), Alias("patreon", "patreon ref")]
        public async Task PatreonRefreshAsync()
        {
            // Check Patreon Refresh cooldown
            // Early return if cooldown

            // Get data from Patreon

            // Compare IDs

            // Add Patron role to users

            // Find user's who are not paying anymore - Delete their Patron rewards

            // Check if it's month since last rewards if yes then distribute them again
        }

        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("patreon -f refresh")]
        public async Task PatreonForceRefreshAsync()
        {
            // Get data from Patreon

            // Compare IDs

            // Add Patron role to users

            // Find user's who are not paying anymore - Delete their Patron rewards

            // Check if it's month since last rewards if yes then distribute them again
        }

        [Command("register")]
        public async Task RegisterAsync()
        {
            User user = User.Get(Context.User.Id);

            if (user != null)
            {
                await Context.Channel.SendMessageAsync(":x: You are already registered!");
                return;
            }

            User.NewRecord(new User(Context.User.Id));
            Inventory.NewRecord(new Inventory(Context.User.Id));
            await Context.Channel.SendMessageAsync(":tada: You have been sucessfully registered!");
        }

        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("suggestions")]
        public async Task SuggestionsAsync()
        {
            _ = Context.Channel.SendMessageAsync(
                "📧 Any ideas or suggestions how to improve something?\n" +
                "We value your feedback and this is perfect place to let us know what you think!");
        }

        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("socialmedia")]

        public async Task SocialMediaAsync()
        {
            string sFilePath = ImageManager.Singleton.FilePath;
            EmbedBuilder embedIG = new EmbedBuilder();
        }

        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("rules")]
        public async Task RulesAsync()
        {
            SocketTextChannel channel = Context.Guild.GetChannel(598895911061684234) as SocketTextChannel;
            await channel.SendMessageAsync("" +
                "__**General rules:**__\n" +
                "**1. Be cool, kind, and civil.** Treat all members with respect and express your thoughts in a constructive manner.\n" +
                "**2. Use English only.** Communicate in English, but be considerate of all languages.\n" +
                "**3. Use an appropriate name and avatar.** Avoid special characters, emoji, obscenities, and impersonation.\n" +
                "**4. Do not spam.** Avoid excessive messages, images, formatting, emoji, commands, and @mentions.\n" +
                "**5. Do not @mention or direct message Eunoki Team.** Respect their time, they are people too.\n" +
                "**6. No self-promotion or advertisements.** This includes unsolicited references and links to other social media, servers, communities, and services in chat or direct messages.\n" +
                "**7. No personal information or doxing.** Protect your privacy and the privacy of others.\n" +
                "**8. No harassment, abuse, or bullying.**\n" +
                "**9. No racist, sexist, anti-LGBTQ+, or otherwise offensive content.**\n" +
                "**10. No political or religious topics.** These complex subjects result in controversial and offensive posts.\n" +
                "**11. No piracy, sexual, NSFW, or otherwise suspicious content.** We do not condone illegal or suspicious discussions and activity.\n" +
                "**12. Rules are subject to common sense.** These rules are not comprehensive and use of loopholes to violate the spirit of these rules is subject to enforcement.\n" +
                "**13. Discord Terms of Service and Community Guidelines apply.** You must be at least 13 years old to use Discord, and abide by all other terms and guidelines.\n" +
                "**https://discordapp.com/guidelines** \n" +
                "**https://discordapp.com/terms**");
            
        }
    }
}
