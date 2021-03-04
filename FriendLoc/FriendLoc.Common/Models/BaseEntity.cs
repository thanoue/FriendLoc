using System;
using Newtonsoft.Json;

namespace FriendLoc.Common.Models
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }

        public double Created { get; set; }
        [JsonIgnore]
        [LiteDB.BsonIgnore]
        public DateTime CreatedTime => Created.TotalSecondsToLocalTime();

        public double Updated { get; set; }
        [LiteDB.BsonIgnore]
        [JsonIgnore]
        public DateTime UpdatedTime => Updated.TotalSecondsToLocalTime();

        public bool IsDeleted { get; set; }
    }
}
