using System.Threading.Tasks;
using FriendLoc.Entity;

namespace  FriendLoc.Common.Repositories
{
    public interface ITripLocationRepository : IBaseRepository<TripLocation>
    {
        Task<bool> AddLocation(string tripId, Location location);

    }
}