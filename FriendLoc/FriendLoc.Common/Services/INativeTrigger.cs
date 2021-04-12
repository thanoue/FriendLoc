using System;
using System.Collections.Generic;
using FriendLoc.Common.Models;
using FriendLoc.Model;

namespace FriendLoc.Common.Services
{
    public interface INativeTrigger
    {
        public void Init(object data);
        public void InitMap(string apiKey, double lat, double lng,bool isAddMaker = false);
        public void UpdateCurrentMarkerLocation(Coordinate coordinate);
        public void AddUsers(IList<MapUserModel> users);
    }
}
