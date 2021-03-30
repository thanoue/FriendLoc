using System;
using System.Collections.Generic;

namespace FriendLoc.Entity
{
    public class Trip : BaseEntity
    {
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public IDictionary<string, string> UserIds { get; set; }
        public IDictionary<string, TripLocation> Locations { get; set; }

        public Trip()
        {
            UserIds = new Dictionary<string, string>();
            Locations = new Dictionary<string, TripLocation>();
        }
    }
}
