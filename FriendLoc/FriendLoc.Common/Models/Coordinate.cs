using System;
using Newtonsoft.Json;

namespace FriendLoc.Common.Models
{
    public class Coordinate
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        public Coordinate()
        {
        }
    }
}
