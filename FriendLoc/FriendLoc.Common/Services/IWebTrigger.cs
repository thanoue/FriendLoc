using System;
using FriendLoc.Common.Models;

namespace FriendLoc.Common.Services
{
    public interface IWebTrigger
    {
        public void OnReady();
        public void LocationUpdated(Coordinate coordinate);
    }
}
