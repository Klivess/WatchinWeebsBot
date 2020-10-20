using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WatchinWeebsBot
{
    class Bot
    {
        // D#+ necessary code
        public DiscordClient Client { get; private set; }

        public InteractivityExtension Interactivity { get; private set; }

        public CommandsNextExtension Commands { get; private set; }

        
        public ConfigJson Config { get; private set; }

        private readonly List<DiscordChannel> HourlyMemesChannelList = new List<DiscordChannel>();

        //bruh
        private readonly List<string> LockedChannelsIds;

        public async Task RunAsync()
        {
            //init
            string jsonString = File.ReadAllText("config.json");
            Config = JsonSerializer.Deserialize<ConfigJson>(jsonString);

            IServiceProvider services = new ServiceCollection()
                .AddSingleton(typeof(ConfigJson), Config)
                .BuildServiceProvider();

            var config = new DiscordConfiguration
            {
                
                Token = Config.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { "!" },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true,
                IgnoreExtraArguments = false,
                Services = services
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.GuildMemberAdded += Client_GuildMemberAdded;

            Client.GuildBanAdded += Client_GuildMemberBanned;

            Client.MessageCreated += Client_MessageSent;

            Client.MessageDeleted += Client_MessageDeleted;

            Client.GuildBanRemoved += Client_BanRemoved;

            Client.GuildMemberRemoved += Client_MemberLeave;

            //Commands = Client.UseCommandsNext(commandsConfig);
            //Commands.RegisterCommands<CommandsClass>();
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        async Task OnClientReady(ReadyEventArgs e)
        {
            Console.WriteLine("Bot is up!");
        }

        // TODO: Debug this
        async Task RandomlyPostTheFunny()
        {
            try
            {

                // doesn't work
                Console.WriteLine("posting the funny");
                Random rnd = new Random();
                string rndpath = Directory.GetFiles("memes").ElementAt(rnd.Next(1, Directory.GetFiles("memes").Length));
                await Client.GetGuildAsync(691036170238427186).Result.GetChannel(729099294669275228).SendFileAsync(rndpath, "Here is your meme!");
                await Task.Delay(3600000);
                await RandomlyPostTheFunny();
                //await Client.GetGuildAsync(691036170238427186).Result.GetChannel(729099294669275228).SendFileAsync(rndpath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        async Task PostTheFunny(DiscordChannel a)
        {
            try
            {
                // meme command
                Random rnd = new Random();
                string rndpath = Directory.GetFiles("memes").ElementAt(rnd.Next(1, Directory.GetFiles("memes").Length));
                await a.SendFileAsync(rndpath, "Here is your meme!");
                //await Client.GetGuildAsync(691036170238427186).Result.GetChannel(729099294669275228).SendFileAsync(rndpath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        async Task PostAllTheFunny(MessageCreateEventArgs a)
        {
            try
            {
                await a.Channel.SendMessageAsync("I am sending " + Directory.GetFiles("memes").Length+" memes to you!");
                foreach (string item in Directory.GetFiles("memes"))
                {
                    await a.Guild.GetMemberAsync(a.Author.Id).Result.SendFileAsync(item);
                    await Task.Delay(3000);
                }
            }
            catch (Exception ex)
            {
                await a.Channel.SendMessageAsync(ex.Message);
            }
        }

        async Task UpdateTheFunny(MessageCreateEventArgs a)
        {
            // C:\Users\Server Computer\Desktop\watchin bot\WatchinWeebsBot\WatchinWeebsBot\bin\Debug\netcoreapp3.1\memes
            string[] strcd =
            {
                "C:",
                "Users",
                "Server Computer",
                "Desktop",
                "watchin bot",
                "WatchinWeebsBot",
                "WatchinWeebsBot",
                "bin",
                "Debug",
                "netcoreapp3.1",
                "memes"
            };
            string finalpath = Path.Combine(strcd);
            string strCmdText = "'/C cd "+finalpath+"&& git pull origin master";
            Process.Start("CMD.exe", strCmdText);
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

        /*                 
await e.Guild.GetMemberAsync(e.Author.Id).Result.SendMessageAsync("Here are the commands: \n" +
    "hello nezuko \n" +
    "!nezukoupdateme \n" +
    "!postthefunny \n" +
    "!restart \n" +
    "!quit \n" +
    "!nezban \n" +
    "!showmeallcool \n" +
    "!smsg \n" +
    "nezuko am i cool \n" +
    "nezuko is he cool \n" +
    "!delmsg \n" +
    "make channel nezuko \n" +
    "delete channel nezuko \n" +
    "!addcool \n" +
    "!addcool \n" +
    "!removecool \n" +
    "!trace \n" +
    "!untrace \n");
*/

        public bool tracing = false;
        public static string[] basemain = { "227462990293762049", "239647961502580737", "453826077442179072" };
        public List<string> important = new List<string>(basemain);

        private bool CheckIfCoolPerson(ulong userId)
        {
            // You shouldn't keep member ids in a publically accessible place.
            return Config.ImportantMembers.Values.Contains(userId);
            // Why'd you have this line? As far as I can tell this wouldn't ever return true with the list of Important Members you have.
            //  && a.Author.Id.ToString() != "<some_user_id>"
        }

        async Task Client_MessageSent(MessageCreateEventArgs e)
        {
            try
            {
                if (e.Message.Author.Id.ToString() == "453826077442179072" || e.Message.Author.Id.ToString() == "238327938859270145")
                {
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromName(Client, ":confounded:"));
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromName(Client, ":sweat_drops:"));
                }
                if (e.Message.Author.Id.ToString() == "295440396006326272")
                {
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromUnicode(Client, "🐷"));
                }
                if (e.Message.Author.Id.ToString() == "280355519728975872")
                {
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromName(Client, ":hamburger:"));
                }
                if (e.Message.Content.ToLower().Contains("lmao") || e.Message.Content.ToLower().Contains("lmfao"))
                {
                    await e.Message.Channel.SendMessageAsync("Uh-huh sure you are. You expect ME to believe that you laughed so hard your fucking ass fell off? You expect me to believe an action that only involves you exhaling, made your ASS and area that has an undefined border, mind you. Fell off? from exhaling? Yea, try harder next time. Maybe they’ll understand your communist nazi socialist propaganda.");
                }
                if (e.Message.Content.ToLower().Replace(" ", string.Empty).Contains("klives"))
                {
                    // spying.... XD
                    try
                    {
                        await e.Guild.GetMemberAsync(227462990293762049).Result.SendMessageAsync(e.Message.Author.Username + " said your name in " + e.Message.Channel.Name + " in " + e.Guild.Name +" | "+e.Message.Content);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                // Um, you don't actually do anything with the locked channels as far as I can tell...
                //if (e.Message.Content.ToLower().Contains("lock this chat") )
                //{
                //    if (e.Message.Author.Id.ToString() == "227462990293762049")
                //    {
                //        LockedChannelsIds.Add(e.Message.Channel.Id.ToString());
                //        await e.Channel.SendMessageAsync("Channel has been locked!");
                //    }
                //    else
                //    {
                //        await e.Message.Channel.SendMessageAsync("Only cool people can execute this!.");
                //    }
                //}
                if (e.Message.Content.ToLower().Contains("!addcool") )
                {
                    var guy = e.Message.MentionedUsers.ElementAtOrDefault(0).Id.ToString();
                    if (CheckIfCoolPerson(e))
                    {
                        if (guy == "257204702528274433" && !e.Message.Author.IsBot)
                        {
                            await e.Message.Channel.SendMessageAsync("Stefo isn't going to get cool person.");
                            await e.Guild.GetMemberAsync(227462990293762049).Result.SendMessageAsync(e.Message.Author.Username + " tried giving Stefo cool role. Bad boy.");
                        }
                        else
                        {
                            important.Add(e.Message.MentionedUsers.ElementAtOrDefault(0).Id.ToString());
                            await e.Message.Channel.SendMessageAsync("Added!");
                        }
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
                if (e.Message.Content.ToLower().Contains("unlock this chat") )
                {
                    if (e.Message.Author.Id.ToString() == "227462990293762049")
                    {
                        foreach (string item in LockedChannelsIds)
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
                if (e.Message.Content.ToLower().Contains("!hourlymeme"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        HourlyMemesChannelList.Add(e.MentionedChannels.ElementAt(0));
                        await e.Message.RespondAsync("Added: " + e.MentionedChannels.ElementAt(0).Name + " to the list!");
                    }
                }
                if (e.Message.Content.ToLower().Contains("!memelist"))
                {
                    string str = "";
                    foreach(var item in HourlyMemesChannelList)
                    {
                        str = str + item.Name + Environment.NewLine;
                    }
                    await e.Message.RespondAsync("Channels currently being memed:" + Environment.NewLine+str);
                }
                if (tracing && e.Message.Author.Id.ToString() == "227462990293762049" && !e.Message.Content.Contains("!trace") && !e.Message.Content.Contains("!untrace") )
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
