using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.EventHandling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WatchinWeebsBot
{
    class Bot
    {

        public DiscordClient Client { get; private set; }

        public InteractivityExtension Interactivity { get; private set; }

        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {

            var config = new DiscordConfiguration
            {
                Token = "NzM3MzI2NTY4MjcxMTE4NDU3.Xx7u4A.osWmyAtgySjuH2ercCtW9zgcp9w",
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.GuildMemberAdded += Client_GuildMemberAdded;

            Client.GuildBanAdded += Client_GuildMemberBanned;

            Client.MessageCreated += Client_MessageSent;

            Client.MessageDeleted += Client_MessageDeleted;

            Client.GuildBanRemoved += Client_BanRemoved;

            Client.GuildMemberRemoved += Client_MemberLeave;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { "!" },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true,
                IgnoreExtraArguments = false
            };

            //Commands = Client.UseCommandsNext(commandsConfig);
            //Commands.RegisterCommands<CommandsClass>();
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        async Task OnClientReady(ReadyEventArgs e)
        {
            Console.WriteLine("Bot is up!");
        }

        async Task Client_GuildMemberBanned(GuildBanAddEventArgs e)
        {
            await e.Guild.GetChannel(691036573713563758).SendMessageAsync(e.Member.Mention + " has been banned.")
                .Result.CreateReactionAsync(DiscordEmoji.FromUnicode("👋"));
        }

        async Task Client_GuildMemberAdded(GuildMemberAddEventArgs e)
        {
            await e.Guild.GetChannel(691036573713563758).SendMessageAsync("A new weeb has joined! Welcome " + e.Member.Mention + "!")
                .Result.CreateReactionAsync(DiscordEmoji.FromUnicode("👋"));
        }

        async Task Client_MessageSent(MessageCreateEventArgs e)
        {
            if (e.Message.Content.ToLower().Contains("hello nezuko"))
            {
                await e.Channel.SendMessageAsync("Hello!");
            }
            if(e.Message.Content.ToLower().Contains("nezuko make role"))
            {
                string name = e.Message.Content.Replace("nezuko make role", string.Empty);
                var role = e.Guild.CreateRoleAsync(name, null, DiscordColor.Blurple);
                var roleid = role.Id;
                await e.Guild.GetMemberAsync(e.Message.Author.Id).Result.GrantRoleAsync();
            }
        }

        async Task Client_MessageDeleted(MessageDeleteEventArgs e)
        {

        }

        async Task Client_BanRemoved(GuildBanRemoveEventArgs e)
        {

        }

        async Task Client_MemberLeave(GuildMemberRemoveEventArgs e)
        {

        }
    }
}
