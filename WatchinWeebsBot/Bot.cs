using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.EventHandling;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WatchinWeebsBot
{
    class Bot
    {

        public DiscordClient Client { get; private set; }

        public InteractivityExtension Interactivity { get; private set; }

        public CommandsNextExtension Commands { get; private set; }

        List<string> bruh;

        public async Task RunAsync()
        {

            ConfigJson cj = new ConfigJson();
            var config = new DiscordConfiguration
            {
                
                Token = cj.Token,
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
            try
            {
                await e.Guild.GetChannel(691036573713563758).SendMessageAsync(e.Member.Mention + " has been banned.")
                    .Result.CreateReactionAsync(DiscordEmoji.FromUnicode("👋"));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task Client_GuildMemberAdded(GuildMemberAddEventArgs e)
        {
            try
            {
                await e.Guild.GetChannel(691036573713563758).SendMessageAsync("A new weeb has joined! Welcome " + e.Member.Mention + "!").Result.CreateReactionAsync(DiscordEmoji.FromUnicode("👋"));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool tracing = false;
        public static string[] basemain = { "227462990293762049", "239647961502580737" };
        public List<string> important = new List<string>(basemain);

        private bool CheckIfCoolPerson(MessageCreateEventArgs a)
        {
            // Elmo 239647961502580737
            // Klives 227462990293762049
            // Cow 295440396006326272
            foreach (string item in important)
            {
                return item == a.Message.Author.Id.ToString();
            }
            return false;

            //return a.Message.Author.Id.ToString() == "227462990293762049"|| a.Message.Author.Id.ToString() == "239647961502580737";
        }

        async Task Client_MessageSent(MessageCreateEventArgs e)
        {
            try
            {
                /*
            foreach(string b in bruh)
            {
                if (e.Channel.Id.ToString() == b && e.Message.Author.Id.ToString() != "227462990293762049")
                {
                    await e.Message.DeleteAsync();
                }
            }
            */
                if (e.Message.Content.ToLower().Contains("hello nezuko"))
                {
                    await e.Channel.SendMessageAsync("Hello!");
                }
                /*
                if(e.Message.Content.ToLower().Contains("nezuko make role"))
                {
                    string name = e.Message.Content.Replace("nezuko make role", string.Empty);
                    var role = e.Guild.CreateRoleAsync(name, null, DiscordColor.Blurple);
                    await e.Channel.SendMessageAsync("Role has been made!");
                }
                */
                if (e.Message.Content.Contains("!restart"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        await e.Message.Channel.SendMessageAsync("Restarting.");
                        Process.Start("WatchinWeebsBot.exe");
                        System.Environment.Exit(1);
                    }
                    else
                    {
                        await e.Message.Channel.SendMessageAsync("Only cool people can execute this!.");
                    }
                }
                if (e.Message.Content.Contains("!quit"))
                {
                    if (e.Message.Author.Id.ToString() == "227462990293762049")
                    {
                        await e.Message.Channel.SendMessageAsync("Quitting.");
                        System.Environment.Exit(1);
                    }
                    else
                    {
                        await e.Message.Channel.SendMessageAsync("Only Klives can execute this!.");
                    }
                }
                if (e.Message.Content.Contains("!showmeallcool"))
                {
                    foreach(string item in important)
                    {
                        e.Channel.SendMessageAsync(item);
                    }
                }
                if (e.Message.Content.Contains("!smsg"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        await e.Message.MentionedChannels.ElementAtOrDefault(0).SendMessageAsync(e.Message.Content.Replace("!smsg", string.Empty));
                        await e.Message.DeleteAsync();
                    }
                    else
                    {
                        await e.Message.Channel.SendMessageAsync("Only cool people can execute this!.");
                    }
                }
                if (e.Message.Content.ToLower().Contains("nezuko am i cool"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        await e.Channel.SendMessageAsync("Yes!");
                    }
                    else
                    {
                        await e.Channel.SendMessageAsync("No!");

                    }
                }
                if (e.Message.Content.Contains("!delmsg"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        await e.Channel.GetMessageAsync(Convert.ToUInt64(e.Message.Content.Replace("!delmsg ", string.Empty))).Result.DeleteAsync();
                        await e.Message.DeleteAsync();
                    }
                    else
                    {
                        await e.Message.Channel.SendMessageAsync("Only cool people can execute this!.");
                    }
                }
                if (e.Message.Content.ToLower().Contains("make channel nezuko"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        string name = e.Message.Content.Replace("make channel nezuko", string.Empty);
                        await e.Guild.CreateChannelAsync(name, 0);
                        await e.Channel.SendMessageAsync("Channel has been made!");
                    }
                    else
                    {
                        await e.Message.Channel.SendMessageAsync("Only cool people can execute this!.");
                    }
                }
                if (e.Message.Content.ToLower().Contains("delete channel nezuko"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        await e.Message.MentionedChannels.ElementAtOrDefault(0).DeleteAsync();
                        await e.Channel.SendMessageAsync("Channel has been deleted!");
                    }
                    else
                    {
                        await e.Message.Channel.SendMessageAsync("Only cool people can execute this!.");
                    }
                }
                if (e.Message.Content.ToLower().Contains("lock this chat"))
                {
                    if (e.Message.Author.Id.ToString() == "227462990293762049")
                    {
                        bruh.Add(e.Message.Channel.Id.ToString());
                        await e.Channel.SendMessageAsync("Channel has been locked!");
                    }
                    else
                    {
                        await e.Message.Channel.SendMessageAsync("Only cool people can execute this!.");
                    }
                }
                if (e.Message.Content.ToLower().Contains("!addcool"))
                {
                    if (CheckIfCoolPerson(e))
                    {

                        important.Add(e.Message.MentionedUsers.ElementAtOrDefault(0).Id.ToString());
                        await e.Message.Channel.SendMessageAsync("Added!");
                    }
                    else
                    {
                        await e.Message.Channel.SendMessageAsync("Only cool people can execute this!.");
                    }
                }
                if (e.Message.Content.ToLower().Contains("!removecool"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        var guy = e.Message.MentionedUsers.ElementAtOrDefault(0).Id.ToString();
                        if (guy == "227462990293762049")
                        {
                            await e.Message.Channel.SendMessageAsync("You can't remove my owner, silly!");
                        }
                        else
                        {
                            var index = important.FindIndex(x => x.Contains(guy));
                            important.RemoveAt(index);
                            await e.Message.Channel.SendMessageAsync("Removed!");
                        }
                    }
                    else
                    {
                        await e.Message.Channel.SendMessageAsync("Only cool people can execute this!.");
                    }
                }
                if (e.Message.Content.ToLower().Contains("unlock this chat"))
                {
                    if (e.Message.Author.Id.ToString() == "227462990293762049")
                    {
                        foreach (string item in bruh)
                        {
                            if (item == e.Channel.Id.ToString())
                            {

                            }
                        }
                    }
                }
                if (e.Message.Content.ToLower().Contains("!trace"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        tracing = true;
                        await e.Message.DeleteAsync();
                    }
                }
                if (e.Message.Content.ToLower().Contains("!untrace"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        tracing = true;
                        await e.Message.DeleteAsync();
                    }
                }
                if (tracing && e.Message.Author.Id.ToString() == "227462990293762049" && !e.Message.Content.Contains("!trace") && !e.Message.Content.Contains("!untrace"))
                {
                    await e.Message.Channel.SendMessageAsync(e.Message.Content);
                    await e.Message.DeleteAsync();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task Client_MessageDeleted(MessageDeleteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task Client_BanRemoved(GuildBanRemoveEventArgs e)
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task Client_MemberLeave(GuildMemberRemoveEventArgs e)
        {
            try
            {
                await e.Guild.GetChannel(691036573713563758).SendMessageAsync("A person has left. Goodbye" + e.Member.Mention + "!").Result.CreateReactionAsync(DiscordEmoji.FromUnicode("👋"));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
