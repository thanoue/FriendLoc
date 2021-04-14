using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FriendLoc.Entity;

namespace FriendLoc.Common.Repositories
{
    public class TripRepository : BaseRepo<Trip>, ITripRepository
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
                if (trip.UserIds == null || trip.UserIds.Count <= 0)
                    continue;

                if (trip.UserIds.ContainsKey(userId))
                    trips.Add(trip);
            }
            return trips;
        }

        public async Task<bool> AddMember(string tripId, string userId)
        {
            return await Handle<bool>(async () =>
            {
               var trip = await GetById(tripId);

               if (trip == null)
               {
                   UtilUI.ErrorToast("Trip Data is not Found!");
                   return false;
               }

               if (trip.UserIds == null)
                   trip.UserIds = new Dictionary<string, string>();

               trip.UserIds.Add(userId, userId);

               return await Client.Child(Path).Child(tripId).Child(nameof(Trip.UserIds)).PutAsync<IDictionary<string, string>>(trip.UserIds)
                   .ContinueWith((res) => { return !res.IsFaulted; });
            });
        }

        public override async Task<Trip> InsertAsync(Trip entity)
        {
            entity.UserIds = new Dictionary<string, string>();

            entity.UserIds.Add(entity.OwnerId, entity.OwnerId);

            return await base.InsertAsync(entity);
        }
    }
}