using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            await RandomlyPostTheFunny();
        }

        async Task RandomlyPostTheFunny()
        {
            try
            {
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

        async Task PostTheFunny(MessageCreateEventArgs a)
        {
            Random rnd = new Random();
            string rndpath = Directory.GetFiles("memes").ElementAt(rnd.Next(1, Directory.GetFiles("memes").Length));
            await a.Channel.SendFileAsync(rndpath, "Here is your meme!");
            //await Client.GetGuildAsync(691036170238427186).Result.GetChannel(729099294669275228).SendFileAsync(rndpath);
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

        public bool tracing = false;
        public static string[] basemain = { "227462990293762049", "239647961502580737", "453826077442179072" };
        public List<string> important = new List<string>(basemain);

        private bool CheckIfCoolPerson(MessageCreateEventArgs a)
        {
            // Elmo 239647961502580737
            // Klives 227462990293762049
            // Cow 295440396006326272
            return important.Contains(a.Author.Id.ToString()) && a.Author.Id.ToString() != "737326568271118457";

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
                if (e.Message.Content.ToLower().Contains("hello nezuko") && e.Message.Author.Id.ToString() == "737326568271118457")
                {
                    await e.Channel.SendMessageAsync("Hello!");
                }
                if (e.Message.Content.ToLower().Contains("!nezukoupdatememe") && CheckIfCoolPerson(e) && e.Message.Author.Id.ToString() == "737326568271118457")
                {
                    await e.Channel.SendMessageAsync("Updating my memes!");
                    await UpdateTheFunny(e);
                }
                if (e.Message.Content.ToLower().Contains("!commands"))
                {
                    await e.Channel.SendMessageAsync("Here are the commands: \n" +
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

                }
                /*
                if(e.Message.Content.ToLower().Contains("nezuko make role"))
                {
                    string name = e.Message.Content.Replace("nezuko make role", string.Empty);
                    var role = e.Guild.CreateRoleAsync(name, null, DiscordColor.Blurple);
                    await e.Channel.SendMessageAsync("Role has been made!");
                }
                */
                if (e.Message.Author.Id.ToString() == "453826077442179072" || e.Message.Author.Id.ToString() == "238327938859270145" && !e.Message.Author.IsBot)
                {
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromName(Client, ":confounded:"));
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromName(Client, ":sweat_drops:"));
                }
                if (e.Message.Content.ToLower().Contains("klives") && !e.Message.Author.IsBot)
                {
                    try
                    {
                        await e.Guild.GetMemberAsync(227462990293762049).Result.SendMessageAsync(e.Message.Author.Username + " said your name in " + e.Message.Channel.Name + " in " + e.Guild.Name);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (e.Message.Content.ToLower().Contains("!postthefunny") && !e.Message.Author.IsBot)
                {
                    await PostTheFunny(e);
                }
                if (e.Message.Content.Contains("!restart") && !e.Message.Author.IsBot)
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
                if (e.Message.Content.ToLower().Contains("!nezban") && !e.Message.Author.IsBot)
                {
                    if (CheckIfCoolPerson(e))
                    {
                        await e.Guild.GetMemberAsync(e.Message.MentionedUsers.ElementAtOrDefault(0).Id).Result.BanAsync();
                        await e.Message.Channel.SendMessageAsync(e.Message.MentionedUsers.ElementAtOrDefault(0).Username);
                    }
                }
                if (e.Message.Content.Contains("!quit") && !e.Message.Author.IsBot)
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
                if (e.Message.Content.Contains("!showmeallcool") && e.Message.Author.Id.ToString() == "737326568271118457")
                {
                    foreach(string item in important)
                    {
                        await e.Channel.SendMessageAsync(item);
                    }
                }
                if (e.Message.Content.Contains("!smsg") && !e.Message.Author.IsBot)
                {
                    if (CheckIfCoolPerson(e))
                    {
                        var firstchaannel = e.Message.MentionedChannels.ElementAtOrDefault(0).Name.ToString();
                        var firstchannelbyid = e.Guild.GetChannel(e.Message.MentionedChannels.ElementAtOrDefault(0).Id);
                        var finalsay = e.Message.Content.ToString().Replace("!smsg ", string.Empty).Replace("#", string.Empty).Replace(firstchaannel, string.Empty).Replace("<", string.Empty).Replace(">", string.Empty).Replace(e.Message.MentionedChannels.ElementAtOrDefault(0).Id.ToString(), string.Empty);
                        await e.Message.DeleteAsync();
                        Console.WriteLine(finalsay);
                        await firstchannelbyid.SendMessageAsync(finalsay);
                    }
                    else
                    {
                        await e.Message.Channel.SendMessageAsync("Only cool people can execute this!.");
                    }
                }
                if (e.Message.Content.ToLower().Contains("nezuko am i cool") && !e.Message.Author.IsBot)
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
                if (e.Message.Content.ToLower().Contains("nezuko is he cool") && !e.Message.Author.IsBot)
                {
                    if (important.Contains(e.Message.MentionedUsers.ElementAtOrDefault(0).Id.ToString()))
                    {
                        await e.Channel.SendMessageAsync("Yes!");
                    }
                    else
                    {
                        await e.Channel.SendMessageAsync("No!");
                    }
                }
                if (e.Message.Content.Contains("!delmsg") && !e.Message.Author.IsBot)
                {
                    await e.Channel.GetMessageAsync(Convert.ToUInt64(e.Message.Content.Replace("!delmsg ", string.Empty))).Result.DeleteAsync();
                    await e.Message.DeleteAsync();
                }
                if (e.Message.Content.ToLower().Contains("make channel nezuko") && !e.Message.Author.IsBot)
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
                if (e.Message.Content.ToLower().Contains("delete channel nezuko") && !e.Message.Author.IsBot)
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
                if (e.Message.Content.ToLower().Contains("lock this chat") && !e.Message.Author.IsBot)
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
                if (e.Message.Content.ToLower().Contains("!addcool") && !e.Message.Author.IsBot)
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
                if (e.Message.Content.ToLower().Contains("!removecool") && !e.Message.Author.IsBot)
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
                if (e.Message.Content.ToLower().Contains("unlock this chat") && !e.Message.Author.IsBot)
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
                if (e.Message.Content.ToLower().Contains("!trace") && !e.Message.Author.IsBot)
                {
                    if (CheckIfCoolPerson(e))
                    {
                        tracing = true;
                        await e.Message.DeleteAsync();
                    }
                }
                if (e.Message.Content.ToLower().Contains("!untrace") && !e.Message.Author.IsBot)
                {
                    if (CheckIfCoolPerson(e))
                    {
                        tracing = true;
                        await e.Message.DeleteAsync();
                    }
                }
                if (tracing && e.Message.Author.Id.ToString() == "227462990293762049" && !e.Message.Content.Contains("!trace") && !e.Message.Content.Contains("!untrace") && !e.Message.Author.IsBot)
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
