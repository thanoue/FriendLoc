using System;
using FriendLoc.Entity;

namespace FriendLoc.Droid.ViewModels
{
    public class TripViewModel : Java.Lang.Object
    {
        public string Name { get; set; }
        public string AvtUrl { get; set; }
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string Milestones { get; set; }
        public string DateRange { get; set; }
        public TripStatuses Status { get; set; }

        public TripViewModel()
        {
        }
    }
}
