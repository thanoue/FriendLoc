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

        public async Task AddLocation(string tripId, Location location)
        {
            try
            {
                await Client.Child(Path).Child(tripId).Child(nameof(TripLocation.Locations)).PostAsync(location);
            }
            catch (FirebaseAuthException firebaseExcep)
            {
                if (firebaseExcep.Reason != AuthErrorReason.InvalidAccessToken)
                    return;

                var refreshToken = await ServiceInstances.AuthService.Login(UserSession.Instance.LoggedinUser.LoginName, UserSession.Instance.LoggedinUser.Password, (err) =>
                {

                });

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await AddLocation(tripId, location);
                }
                else
                    return;
            }
        }
    }
}
