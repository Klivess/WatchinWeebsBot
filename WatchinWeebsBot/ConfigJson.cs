using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WatchinWeebsBot
{
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        public string Prefix { get; private set; }
    }
}
