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

        public async Task<bool> AddMileStone(string userId, Milestone milestone)
        {
            return await Handle<bool>(async () =>
            {
                return await Client.Child(Path).Child(userId).Child(nameof(UserMilestone.Milestones)).PostAsync(milestone)
                        .ContinueWith((res) => { return !res.IsFaulted; });
            });
        }
    }
}