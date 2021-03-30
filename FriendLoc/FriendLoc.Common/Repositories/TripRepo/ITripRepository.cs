using System;
using System.Threading.Tasks;
using FriendLoc.Entity;

namespace FriendLoc.Common.Repositories
{
    public interface ITripRepository : IBaseRepository<Trip> 
    {
        Task AddLocation(string tripId, TripLocation location);
    }
}
