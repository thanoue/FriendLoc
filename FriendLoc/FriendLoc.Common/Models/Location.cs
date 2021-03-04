using System;
using LiteDB;
using Newtonsoft.Json;

namespace FriendLoc.Common.Models
{
    public class TripLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string UserId { get; set; }
        public string TripId { get; set; }

        public double Created { get; set; }
        [JsonIgnore]
        [BsonIgnore]
        public DateTime CreatedTime => Created.TotalSecondsToLocalTime();

        public TripLocation()
        {
        }
    }
}
