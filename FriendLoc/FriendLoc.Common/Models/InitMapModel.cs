using System;
using Newtonsoft;
using Newtonsoft.Json;

namespace FriendLoc.Model
{
    public class InitMapModel
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        public InitMapModel()
        {
        }
    }
}
