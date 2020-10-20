using System.Collections.Generic;

namespace WatchinWeebsBot
{
    public class ConfigJson
    {
        public string Token { get; set; }

        public Dictionary<string, ulong> ImportantMembers { get; set; }

        public Dictionary<string, ulong> RoleIds { get; set; }

        public Dictionary<string, ulong> Users { get; set; }

        public bool Trace { get; set; }
    }
}