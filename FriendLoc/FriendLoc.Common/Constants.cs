using System;
namespace FriendLoc.Common
{
    public static class Constants
    {
        public const string APP_NAME = "FriendLoc";

        public const string TripId = "TripId";
        public const string UserId = "UserId";

        public const string MapUrl = "http://192.168.2.167:5000";

      
    }

    public class Command
    {
        public const string documentIsReady = "documentIsReady";
        public const string addNewUser = "addNewUser";
        public const string addUserList = "addUserList";
        public const string updaterUserLocation = "updaterUserLocation";
        public const string removeUser = "removeUser";
        public const string initMap = "initMap";
    }
}
