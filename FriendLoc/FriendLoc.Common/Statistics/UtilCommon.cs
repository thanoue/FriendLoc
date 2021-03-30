using System;
using Firebase.Database;
using Newtonsoft.Json;
using FriendLoc.Entity;
using FriendLoc.Model;

namespace FriendLoc.Common
{
    public static class UtilCommon
    {
        public static double GetLocalTimeTotalSeconds(this DateTime localtime)
        {
            return localtime.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalSeconds; 
        }

        public static DateTime TotalSecondsToLocalTime(this double totalUtcSeconds)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dtfommls = dt.AddSeconds(totalUtcSeconds);

            return dtfommls.ToLocalTime();
        }

        public static string FirebaseAuthExceptionHandler(string exc)
        {
            var model = JsonConvert.DeserializeObject<FirebaseClientException>(exc);

            switch (model.Error.Message)
            {
                case "EMAIL_EXISTS":

                    return "The email address is already in use by another account!";
                case "OPERATION_NOT_ALLOWED":

                    return "Password sign-in is disabled for this project!";

                case "TOO_MANY_ATTEMPTS_TRY_LATER":

                    return "We have blocked all requests from this device due to unusual activity. Try again later!";

                case "INVALID_PASSWORD":

                    return "Invalid Password!";
                    
                case "EMAIL_NOT_FOUND":

                    return "Email is not found!";

                default:

                    return "Has some errors with Firebase Authentication";
            }
        }
    }
}
