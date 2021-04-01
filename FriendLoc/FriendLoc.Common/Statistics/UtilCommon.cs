using System;
using Firebase.Database;
using Newtonsoft.Json;
using FriendLoc.Entity;
using FriendLoc.Model;
using Firebase.Auth;

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

        public static string FirebaseAuthExceptionHandler(FirebaseAuthException firebaseExcep)
        {
            switch (firebaseExcep.Reason)
            {
                case AuthErrorReason.EmailExists:
                    return "The email address is already in use by another account!";

                case AuthErrorReason.TooManyAttemptsTryLater:

                    return "We have blocked all requests from this device due to unusual activity. Try again later!";

                case AuthErrorReason.InvalidEmailAddress:
                case AuthErrorReason.UnknownEmailAddress:

                    return "Email is not found!";

                case AuthErrorReason.OperationNotAllowed:

                   return  "Password sign-in is disabled for this project!";

                case AuthErrorReason.WrongPassword:

                    return "Invalid Password!";

                default:

                    return "Has some errors with Firebase Authentication";

            }
        }
    }
}
