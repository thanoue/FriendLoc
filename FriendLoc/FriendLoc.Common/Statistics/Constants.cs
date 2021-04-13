using System;
namespace FriendLoc.Common
{
    public static class Constants
    {
        public const string APP_NAME = "FriendLoc";

        public const string TripId = "TripId";
        public const string UserId = "UserId";

        //public const string MapUrl = "http://192.168.1.34:5000";
        public const string MapUrl = "https://friendloc-98ed3.web.app/";

        public const string FirebaseApiKey = "AIzaSyCxjza0PW9fg6y4tPlljkP-iBSwOC0XY6g";
        public const string FirebaseDbPath = "https://friendloc-98ed3-default-rtdb.firebaseio.com/";
        public const string FirebaseStoragePath = "friendloc-98ed3.appspot.com";
        public const string HereMapApiKey = "FJqTTDyUrZA2WiGRgXWfkWEfdK98VSCgHpGpn1bkvMM";
        public const string EmailSuffix = "@friendloc.com";

        public const string UserAvtStorageFolderName = "UserAvt";
        public const string MileStoneStorageFolderName = "MileStone";

        public const string UserToken = "UserToken";
        public const string LoggedinUser = "LoggedinUser";
        public const string LastestLoggedIn = "LastestLoggedIn";
        public const int LoginNameMaxLength = 15;
    }

    public class Command
    {
        public const string documentIsReady = "documentIsReady";
        public const string addNewUser = "addNewUser";
        public const string addUserList = "addUserList";
        public const string updaterUserLocation = "updaterUserLocation";
        public const string removeUser = "removeUser";
        public const string initMap = "initMap";
        public const string locationUpdated = "locationUpdated";
        public const string addCoordinate = "addCoordinate";
        public const string locationChaningRequest = "locationChaningRequest";
    }
}
