using System;
using System.Collections.Generic;
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
                return await Client.Child(Path).Child(userId).Child(nameof(UserMilestone.Milestones))
                    .PostAsync(milestone)
                    .ContinueWith((res) => { return !res.IsFaulted; });
            });
        }

        public async Task<IList<Milestone>> GetAllByUser(string userId)
        {
            return await Handle<IList<Milestone>>(async () =>
            {
                var userMileStone = await Client.Child(Path).Child(userId).OnceSingleAsync<UserMilestone>();

                if (userMileStone.Milestones.Count <= 0)
                    return null;

                var milestones = new List<Milestone>();

                foreach (var milestone in userMileStone.Milestones)
                {
                    milestone.Value.Id = milestone.Key;
                    milestones.Add(milestone.Value);
                }

                return milestones;
            });
        }
    }
}