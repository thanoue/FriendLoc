using System;
using System.Threading.Tasks;
using FriendLoc.Entities;

namespace FriendLoc.Common.Repositories
{
    public interface IUserMilestoneRepository : IBaseRepository<UserMilestone> 
    {
        Task AddMileStone(string userId, Milestone milestone);
    }
}
