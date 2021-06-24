using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FriendLoc.Entities;

namespace FriendLoc.Common.Repositories
{
    public interface IUserMilestoneRepository : IBaseRepository<UserMilestone> 
    {
        Task<bool> AddMileStone(string userId, Milestone milestone);

        Task<IList<Milestone>> GetAllByUser(string userId);
    }
}
