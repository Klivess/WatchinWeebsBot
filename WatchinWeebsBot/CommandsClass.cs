using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchinWeebsBot
{
    public class CommandsClass : BaseCommandModule
    {
        private readonly ConfigJson config;
        private readonly ILogger logger;

        public CommandsClass(ConfigJson config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        [Command("hello nezuko")]
        public async Task HelloNezuko(CommandContext context)
        {
            if (IsSpecifiedUser(context.Member.Id, config.SomeRandomUserId))
            {
                return;
            }
            await context.RespondAsync("Hello!");
        }

        [Command("postallthefunny")]
        public async Task PostAllTheFunnies(CommandContext context)
        {
            logger.LogInformation("Someone wants all of the memes!");


            await context.RespondAsync("I am sending " + Directory.GetFiles("memes").Length + " memes to you!");
            foreach (string item in Directory.GetFiles("memes"))
            {
                await context.Member.SendFileAsync(item);
                await Task.Delay(3000);
            }
        }

        [Command("amongus")]
        public async Task GrantAmongUsRole(CommandContext context)
        {
            await context.Member.GrantRoleAsync(context.Guild.GetRole(config.RoleIds["AmongUs"]));
            await context.RespondAsync("Gave you the Among Us role!");
        }

        [Command("destiny")]
        public async Task GrantDestinyRole(CommandContext context)
        {
            await context.Member.GrantRoleAsync(context.Guild.GetRole(config.RoleIds["Destiny"]));
            await context.RespondAsync("Gave you the Destiny role!");
        }

        [Command("nezleave")]
        public async Task NezLeave(CommandContext context)
        {
            if (!IsSpecifiedUser(context.Member.Id, config.ImportantMembers["Elmo"]))
            {
                return;
            }

            await context.RespondAsync("I am leaving now, cya!");
            await context.Guild.LeaveAsync();
        }

        [Command("nezleave")]
        public async Task NezLeave(CommandContext context, ulong guildId)
        {
            if (!IsSpecifiedUser(context.Member.Id, config.ImportantMembers["Elmo"]))
            {
                return;
            }
            
            if (context.Client.Guilds.ContainsKey(guildId))
            {
                DiscordGuild guild = context.Client.Guilds[guildId];
                await context.RespondAsync($"I am leaving {guild.Name}");
                await guild.LeaveAsync();
            }
            else
            {
                await context.RespondAsync("I couldn't find that guild!");
            }
        }

        [Command("nezping")]
        public async Task NezPing(CommandContext context)
        {
            await context.RespondAsync($"Ping: {context.Client.Ping} ms");
        }

        [Command("terrariaplayers")]
        public async Task TerrariaPlayers(CommandContext context)
        {
            logger.LogInformation("Someone wants terraria players!");
            // this is fucking terrible code...
            Process[] localByName = Process.GetProcessesByName("Terraria");
            StreamWriter mystream = localByName.ElementAtOrDefault(0).StandardInput;
            StreamReader mystream2 = localByName.ElementAtOrDefault(0).StandardOutput;
            mystream.WriteLine("playing");
            await context.RespondAsync(mystream2.ReadLine());
        }

        [Command("giverole")]
        public async Task GiveRole(CommandContext context, DiscordMember member, DiscordRole role)
        {
            if (!config.ImportantMembers.ContainsValue(context.Member.Id))
            {
                await context.RespondAsync("You must be cool to use this!");
                return;
            }

            await member.GrantRoleAsync(role);
            await context.RespondAsync("Granted role!");
        }

        [Command("removerole")]
        public async Task RemoveRole(CommandContext context, DiscordMember member, DiscordRole role)
        {
            if (!config.ImportantMembers.ContainsValue(context.Member.Id))
            {
                await context.RespondAsync("You must be cool to use this!");
                return;
            }

            await member.RevokeRoleAsync(role);
            await context.RespondAsync("Revoked role!");
        }

        [Command("postthefunny")]
        public async Task PostTheFunny(CommandContext context)
        {
            // meme command
            Random rnd = new Random();
            string rndpath = Directory.GetFiles("memes").ElementAt(rnd.Next(1, Directory.GetFiles("memes").Length));
            await context.RespondWithFileAsync(rndpath, "Here is your meme!");
        }

        [Command("restart")]
        public async Task RestartBot(CommandContext context)
        {
            if (!config.ImportantMembers.ContainsValue(context.Member.Id))
            {
                await context.RespondAsync("Only cool people can execute this!.");
                return;
            }

            await context.RespondAsync("Restarting!");
            Process.Start("WatchinWeebsBot.exe");
            System.Environment.Exit(1);
        }

        [Command("nezban")]
        public async Task BanMember(CommandContext context, DiscordMember member)
        {
            if (!config.ImportantMembers.ContainsValue(context.Member.Id))
            {
                await context.RespondAsync("Only cool people can execute this!.");
                return;
            }

            await member.BanAsync();
            await context.RespondAsync($"{member.Username} has been banned");
        }

        [Command("quit")]
        public async Task QuitBot(CommandContext context)
        {
            if (!IsSpecifiedUser(context.Member.Id, config.ImportantMembers["Klives"]))
            {
                await context.RespondAsync("Only Klives can execute this!");
                return;
            }

            await context.RespondAsync("Quitting!");
            System.Environment.Exit(1);
        }

        [Command("showmeallcool")]
        public async Task ShowAllCoolMembers(CommandContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var importantMemberId in config.ImportantMembers.Values)
            {
                DiscordUser importantMember = await context.Client.GetUserAsync(importantMemberId);
                stringBuilder.AppendLine(importantMember.Username);
            }

            await context.RespondAsync(stringBuilder.ToString());
        }

        [Command("smsg")]
        public async Task SendMessageToChannel(CommandContext context, DiscordChannel channel, [RemainingText] string message)
        {
            if (!config.ImportantMembers.ContainsValue(context.Member.Id))
            {
                await context.RespondAsync("Only cool people can execute this!.");
                return;
            }

            await channel.SendMessageAsync(message);
        }

        [Command("nezuko am i cool")]
        public async Task IsUserCool(CommandContext context)
        {
            if (config.ImportantMembers.ContainsValue(context.Member.Id))
            {
                await context.RespondAsync("Yes!");
            }
            else
            {
                await context.RespondAsync("No!");
            }
        }

        [Command("nezuko is he cool")]
        public async Task IsMentionedUserCool(CommandContext context, DiscordMember member)
        {
            if (config.ImportantMembers.ContainsValue(member.Id))
            {
                await context.RespondAsync("Yes!");
            }
            else
            {
                await context.RespondAsync("No!");
            }
        }

        [Command("delmsg")]
        public async Task DeleteMessageById(CommandContext context, ulong msgId)
        {
            // You currently allow anyone to delete any messages that your bot can delete? Is this intended?
            if (!config.ImportantMembers.ContainsValue(context.Member.Id))
            {
                await context.RespondAsync("Only cool people can execute this!.");
                return;
            }

            DiscordMessage discordMessage = await context.Channel.GetMessageAsync(msgId);
            await context.Channel.DeleteMessageAsync(discordMessage);
        }

        [Command("make channel nezuko")]
        public async Task MakeChannel(CommandContext context, string name)
        {
            if (!config.ImportantMembers.ContainsValue(context.Member.Id))
            {
                await context.RespondAsync("Only cool people can execute this!.");
                return;
            }

            DiscordChannel channel = await context.Guild.CreateChannelAsync(name, DSharpPlus.ChannelType.Text);
            await channel.SendMessageAsync($"{context.Member.Mention}, I've created the channel!");
        }

        [Command("delete channel nezuko")]
        public async Task DeleteChannel(CommandContext context, DiscordChannel channel)
        {
            if (!config.ImportantMembers.ContainsValue(context.Member.Id))
            {
                await context.RespondAsync("Only cool people can execute this!.");
                return;
            }

            await channel.DeleteAsync();
            await context.RespondAsync("Channel has been deleted");
        }




        private bool IsSpecifiedUser(ulong userId, ulong specifiedUserId)
        {
            if (userId == specifiedUserId)
            {
                return true;
            }

            return false;
        }
    }
}
