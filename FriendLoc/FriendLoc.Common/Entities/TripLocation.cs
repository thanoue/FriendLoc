using System;
using System.Collections.Generic;
using FriendLoc.Common;
using FriendLoc.Entities;
using LiteDB;
using Newtonsoft.Json;

namespace FriendLoc.Entity
{
    public class TripLocation : BaseEntity
    {
        public IDictionary<string, Location> Locations { get; set; }

        public TripLocation()
        {
            Locations = new Dictionary<string, Location>();
        }
    }
    public class Location
    {
        public  string UserId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Created { get; set; }
        [JsonIgnore]
        [BsonIgnore]
        public DateTime CreatedTime => Created.TotalSecondsToLocalTime();
        public Location()
        {
        }
    }
}
