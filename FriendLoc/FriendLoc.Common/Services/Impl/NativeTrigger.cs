using System;
using System.Collections.Generic;
using FriendLoc.Common.Models;
using FriendLoc.Common.Services;
using FriendLoc.Model;
using Newtonsoft.Json;

namespace FriendLoc.Common.Services.Impl
{
    public abstract class NativeTrigger : INativeTrigger
    {
        public NativeTrigger()
        {

        }

        public abstract void Init(object data);
        public abstract void Excute(string command,string data);

        public void InitMap(string apiKey, double lat, double lng, bool isAddMaker = false)
        {
            var res = JsonConvert.SerializeObject(new InitMapModel()
            {
                IsAddMarker = isAddMaker,
                ApiKey = apiKey,    
                Lat = lat,
                Lng = lng
            });

            Excute(Command.initMap, res);
        }

        public void AddUsers(IList<MapUserModel> users)
        {
            Excute(Command.addUserList, JsonConvert.SerializeObject(users));
        }

        public void UpdateCurrentMarkerLocation(Coordinate coordinate)
        {
            Excute(Command.locationChaningRequest, JsonConvert.SerializeObject(coordinate));
        }
    }
}
