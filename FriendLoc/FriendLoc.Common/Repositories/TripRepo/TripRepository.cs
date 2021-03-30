using System;
using System.Threading.Tasks;
using Firebase.Database.Query;
using FriendLoc.Entity;

namespace FriendLoc.Common.Repositories
{
    public class TripRepository : BaseRepo<Trip> , ITripRepository
    {
        public override string Path => "Trips";

        public TripRepository()
        {
        }

        public async Task AddLocation(string tripId,TripLocation location)
        {
            location.Created = DateTime.Now.GetLocalTimeTotalSeconds();
            await Client.Child(Path).Child(tripId).Child(nameof(Trip.Locations)).PostAsync(location);
        }
    }
}
