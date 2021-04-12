using System;
using System.Collections.Generic;
using FriendLoc.Entity;

namespace FriendLoc.Entities
{
    public class UserMilestone : BaseEntity
    {
        public IDictionary<string, Milestone> Milestones { get; set; }

        public UserMilestone()
        {
            Milestones = new Dictionary<string, Milestone>();
        }
    }

    public class Milestone
    {
        public string Name { get; set; }
        public string DisplayImgUrl { get; set; }

        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
