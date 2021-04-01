using System;
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

        public async Task AddLocation(string tripId,TripLocation location)
        {
            try
            {
                location.Created = DateTime.Now.GetLocalTimeTotalSeconds();
                await Client.Child(Path).Child(tripId).Child(nameof(Trip.Locations)).PostAsync(location);
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
