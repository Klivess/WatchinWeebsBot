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
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace WatchinWeebsBot
{
    class Bot
    {
        List<DiscordChannel> dc = new List<DiscordChannel>();
        public DiscordClient Client { get; private set; }

        public InteractivityExtension Interactivity { get; private set; }

        public CommandsNextExtension Commands { get; private set; }

        //bruh
        List<string> bruh;

        public async Task RunAsync()
        {
            //init
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
                // Help me Daddy Doofenshmirtz
                if (e.Message.Content.ToLower().Contains("hello nezuko") && e.Message.Author.Id.ToString() != "737326568271118457")
                {
                    await e.Channel.SendMessageAsync("Hello!");
                }
                if (e.Message.Content.ToLower().Contains("!commands"))
                {
                    await e.Guild.GetMemberAsync(e.Author.Id).Result.SendMessageAsync("Fuck you.");
                }
                if (e.Message.Content.ToLower().Contains("!postallofthefunny"))
                {
                    Console.WriteLine("Someone wants all of the memes!");
                    await PostAllTheFunny(e);
                }
                if (e.Message.Content.ToLower().Contains("!amongus"))
                {
                    await e.Message.Channel.Guild.GetMemberAsync(e.Author.Id).Result.GrantRoleAsync(e.Guild.GetRole(761137011468926996));
                    await e.Message.RespondAsync("Gave you the Among Us role!");
                }
                if (e.Message.Content.ToLower().Contains("!destiny"))
                {
                    await e.Message.Channel.Guild.GetMemberAsync(e.Author.Id).Result.GrantRoleAsync(e.Guild.GetRole(746149388501647410));
                    await e.Message.RespondAsync("Gave you the Destiny role!");
                }
                if (e.Message.Content.ToLower().Contains("nezleave") && e.Message.Author.Id.ToString() == "227462990293762049")
                {
                    if(e.Message.Content.ToLower().Replace("nezleave", string.Empty) == "")
                    {
                        await e.Channel.SendMessageAsync("I am leaving now, cya!");
                        await e.Guild.LeaveAsync();
                    }
                    else
                    {
                        bool foundone = false;
                        foreach(var item in Client.Guilds)
                        {
                            if(item.Value.ToString().Contains(e.Message.Content.ToLower().Replace("nezleave", string.Empty)))
                            {
                                var guild = Client.GetGuildAsync(item.Key);
                                await e.Channel.SendMessageAsync("I left "+guild.Result.Name+"!");
                                await guild.Result.LeaveAsync();
                                foundone = true;
                            }
                        }
                        if (foundone != true)
                        {
                            await e.Channel.SendMessageAsync("Couldn't find that discord!");
                        }
                    }
                }
                if (e.Message.Content.ToLower().Contains("nezping"))
                {
                    await e.Channel.SendMessageAsync("Ping: "+Client.Ping+"ms");
                }
                if (e.Message.Content.ToLower().Contains("!terrariaplayers"))
                {
                    Console.WriteLine("Someone wants terraria players!");
                    try
                    {
                        // this is fucking terrible code...
                        Process[] localByName = Process.GetProcessesByName("Terraria");
                        StreamWriter mystream = localByName.ElementAtOrDefault(0).StandardInput;
                        StreamReader mystream2 = localByName.ElementAtOrDefault(0).StandardOutput;
                        mystream.WriteLine("playing");
                        await e.Message.Channel.SendMessageAsync(mystream2.ReadLine());
                    }
                    catch(Exception ex)
                    {
                        await e.Message.Channel.SendMessageAsync(ex.Message);
                    }
                }
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
                    try
                    {
                        await e.Guild.GetMemberAsync(227462990293762049).Result.SendMessageAsync(e.Message.Author.Username + " said your name in " + e.Message.Channel.Name + " in " + e.Guild.Name +" | "+e.Message.Content);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (e.Message.Content.ToLower().Contains("!giverole"))
                {
                    try
                    {
                        if (CheckIfCoolPerson(e))
                        {
                            // this is fucking terrible code...
                            //Member 227462990293762049; Klives!#4448 (Klives!)
                            var person = e.Message.Channel.Guild.GetMemberAsync(e.Message.MentionedUsers.ElementAtOrDefault(0).Id);
                            var id = e.Message.Content.Replace("!giverole", string.Empty).Replace("Member", string.Empty)
                                .Replace(e.Message.MentionedUsers.ElementAtOrDefault(0).Id + ";", string.Empty)
                                .Replace(person.Result.Mention.Replace("@", string.Empty), string.Empty)
                                .Replace("(", string.Empty).Replace(")", string.Empty)
                                .Replace(person.Result.DisplayName, string.Empty)
                                .Replace(">", string.Empty).Replace("<", string.Empty).Replace("@!", string.Empty)
                                .Replace(person.Result.Id.ToString(), string.Empty).Trim();
                            Console.WriteLine(id);
                            await person.Result.GrantRoleAsync(e.Guild.GetRole(Convert.ToUInt64(id)));
                            Console.WriteLine(e.Message.MentionedUsers.ElementAtOrDefault(0));
                            await e.Message.Channel.SendMessageAsync("Granted role!");
                        }
                        else
                        {
                            await e.Message.Channel.SendMessageAsync("You must be cool to use this!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await e.Message.Channel.SendMessageAsync(ex.Message);
                    }
                }
                if (e.Message.Content.ToLower().Contains("!removerole"))
                {
                    try
                    {
                        if (CheckIfCoolPerson(e))
                        {
                            // this is fucking terrible code...
                            var person = e.Message.Channel.Guild.GetMemberAsync(e.Message.MentionedUsers.ElementAtOrDefault(0).Id);
                            var id = e.Message.Content.Replace("!removerole", string.Empty).Replace("Member", string.Empty)
                                .Replace(e.Message.MentionedUsers.ElementAtOrDefault(0).Id + ";", string.Empty)
                                .Replace(person.Result.Mention.Replace("@", string.Empty), string.Empty)
                                .Replace("(", string.Empty).Replace(")", string.Empty)
                                .Replace(person.Result.DisplayName, string.Empty)
                                .Replace(">", string.Empty).Replace("<", string.Empty).Replace("@!", string.Empty)
                                .Replace(person.Result.Id.ToString(), string.Empty).Trim();
                            Console.WriteLine(id);
                            await person.Result.RevokeRoleAsync(e.Guild.GetRole(Convert.ToUInt64(id)));
                            Console.WriteLine(e.Message.MentionedUsers.ElementAtOrDefault(0));
                            await e.Message.Channel.SendMessageAsync("Revoked role!");
                        }
                        else
                        {
                            await e.Message.Channel.SendMessageAsync("You must be cool to use this!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await e.Message.Channel.SendMessageAsync(ex.Message);
                    }
                }
                if (e.Message.Content.ToLower().Contains("!postthefunny"))
                {
                    await PostTheFunny(e.Channel);
                }
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
                if (e.Message.Content.ToLower().Contains("!nezban"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        await e.Guild.GetMemberAsync(e.Message.MentionedUsers.ElementAtOrDefault(0).Id).Result.BanAsync();
                        await e.Message.Channel.SendMessageAsync(e.Message.MentionedUsers.ElementAtOrDefault(0).Username);
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
                        await e.Channel.SendMessageAsync(item);
                    }
                }
                if (e.Message.Content.Contains("!smsg"))
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
                if (e.Message.Content.ToLower().Contains("nezuko am i cool") )
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
                if (e.Message.Content.ToLower().Contains("nezuko is he cool"))
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
                if (e.Message.Content.Contains("!delmsg"))
                {
                    await e.Channel.GetMessageAsync(Convert.ToUInt64(e.Message.Content.Replace("!delmsg ", string.Empty))).Result.DeleteAsync();
                    await e.Message.DeleteAsync();
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
                if (e.Message.Content.ToLower().Contains("lock this chat") )
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
                if (e.Message.Content.ToLower().Contains("!hourlymeme"))
                {
                    if (CheckIfCoolPerson(e))
                    {
                        dc.Add(e.MentionedChannels.ElementAt(0));
                        await e.Message.RespondAsync("Added: " + e.MentionedChannels.ElementAt(0).Name + " to the list!");
                    }
                }
                if (e.Message.Content.ToLower().Contains("!memelist"))
                {
                    string str = "";
                    foreach(var item in dc)
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
