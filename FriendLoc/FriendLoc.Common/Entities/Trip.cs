using System;
using System.Collections.Generic;
using FriendLoc.Common;
using LiteDB;
using Newtonsoft.Json;

namespace FriendLoc.Entity
{
    public class Trip : BaseEntity
    {
        public string OwnerId { get;set; }
        public string Name { get; set; }

        public double StartPointLatitude { get; set; }
        public double StartPointLongitute { get; set; }
        public string StartPointName { get; set; }

        public double EndPointLatitude { get; set; }
        public double EndPointLongitute { get; set; }
        public string EndPointName { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public TripStatuses Status { get; set; }

        public double StartTimeValue { get; set; }
        [JsonIgnore]
        [BsonIgnore]
        public DateTime StartTime
        {
            get { return StartTimeValue.TotalSecondsToLocalTime(); }
            set { StartTimeValue = value.GetLocalTimeTotalSeconds(); }
        }


        public double EndTimeValue { get; set; }
        [JsonIgnore]
        [BsonIgnore]
        public DateTime EndTime
        {
            get { return EndTimeValue.TotalSecondsToLocalTime(); }
            set { EndTimeValue = value.GetLocalTimeTotalSeconds(); }
        }

        public double ExecuteTimeValue { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public DateTime ExecuteTime
        {   
            get { return ExecuteTimeValue.TotalSecondsToLocalTime(); }
            set { ExecuteTimeValue = value.GetLocalTimeTotalSeconds(); }
        }
        public IDictionary<string, string> UserIds { get; set; }
        public IDictionary<string, TripLocation> Locations { get; set; }


        public Trip()
        {
            UserIds = new Dictionary<string, string>();
            Locations = new Dictionary<string, TripLocation>();
        }
    }

    public enum TripStatuses
    {
        Created,
        Runnning,
        Completed,
        Canceled,
    }
}
