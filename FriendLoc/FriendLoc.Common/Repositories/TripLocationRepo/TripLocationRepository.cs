using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database.Query;
using FriendLoc.Entities;
using FriendLoc.Entity;

namespace FriendLoc.Common.Repositories
{
    public class TripLocationRepository : BaseRepo<TripLocation>, ITripLocationRepository
    {
        public override string Path => "TripLocations";

        public async Task<bool> AddLocation(string tripId, Location location)
        {
            return await Handle<bool>(async () =>
            {
                return await Client.Child(Path).Child(tripId).Child(nameof(TripLocation.Locations)).PostAsync(location).ContinueWith((res) => { return !res.IsFaulted; });
            });
        }

    }
}
