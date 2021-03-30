using System;
using Newtonsoft.Json;

namespace FriendLoc.Model
{
    public class MapUserModel
    {
        [JsonProperty("avtUrl")]
        public string AvtUrl { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        public MapUserModel()
        {
        }
    }
}
