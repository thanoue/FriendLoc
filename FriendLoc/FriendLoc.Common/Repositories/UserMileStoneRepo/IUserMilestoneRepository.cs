using System;
using System.Threading.Tasks;
using FriendLoc.Entities;

namespace FriendLoc.Common.Repositories
{
    public interface IUserMilestoneRepository : IBaseRepository<UserMilestone> 
    {
        Task<bool> AddMileStone(string userId, Milestone milestone);
    }
}
