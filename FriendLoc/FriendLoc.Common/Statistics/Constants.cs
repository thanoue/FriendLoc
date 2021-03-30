using System;
namespace FriendLoc.Common
{
    public static class Constants
    {
        public const string APP_NAME = "FriendLoc";

        public const string TripId = "TripId";
        public const string UserId = "UserId";

        public const string MapUrl = "http://192.168.2.167:5000";

        public const string FirebaseApiKey = "AIzaSyCxjza0PW9fg6y4tPlljkP-iBSwOC0XY6g";
        public const string FirebaseDbPath = "https://friendloc-98ed3-default-rtdb.firebaseio.com/";
        public const string FirebaseStoragePath = "friendloc-98ed3.appspot.com";

        public const string EmailSuffix = "@friendloc.com";

        public const string UserAvtStorageFolderName = "UserAvt";

        public const string LoginName = "LoginName";
        public const string Password = "Password";

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
    }
}
