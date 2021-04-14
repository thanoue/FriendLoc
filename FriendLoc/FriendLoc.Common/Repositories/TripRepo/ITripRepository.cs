using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FriendLoc.Entity;

namespace FriendLoc.Common.Repositories
{
    public interface ITripRepository : IBaseRepository<Trip>
    {
        Task<IList<Trip>> GetByJoinedUser(string userId);
        Task<bool> AddMember(string tripId, string userId);

    }
}
