using System;
using System.Threading.Tasks;
using Firebase.Database;
using FriendLoc.Entity;

namespace FriendLoc.Common
{
    public class UserSession
    {
        static Lazy<UserSession> _instance = new Lazy<UserSession>();
        public static UserSession Instance = _instance.Value;
        public User LoggedinUser { get; set; }
    }
}
