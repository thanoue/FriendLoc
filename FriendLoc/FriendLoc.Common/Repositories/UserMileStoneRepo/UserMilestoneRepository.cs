using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database.Query;
using FriendLoc.Entities;

namespace FriendLoc.Common.Repositories
{
    public class UserMilestoneRepository : BaseRepo<UserMilestone>, IUserMilestoneRepository
    {
        public override string Path => "UserMilestones";

        public async Task AddMileStone(string userId, Milestone milestone)
        {
            try
            {
                await Client.Child(Path).Child(userId).Child(nameof(UserMilestone.Milestones)).PostAsync(milestone);
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
                    await AddMileStone(userId, milestone);
                }
                else
                    return;
            }
        }
    }
}
