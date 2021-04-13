using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
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
        
        public async Task<IList<Trip>> GetByJoinedUser(string userId)
        {
            var allTrips = await GetAll();
            var trips = new List<Trip>();
            
            foreach (var trip in allTrips)
            {
                if(trip.UserIds == null || trip.UserIds.Count <=0)
                    continue;

                if(trip.UserIds.ContainsKey(userId))
                    trips.Add(trip);
            }

            return trips;
        }
        public override async Task<Trip> InsertAsync(Trip entity)
        {
            entity.UserIds = new Dictionary<string, string>();
            
            entity.UserIds.Add(entity.OwnerId,entity.OwnerId);
            
            return await base.InsertAsync(entity);
        }

     
    }
}
