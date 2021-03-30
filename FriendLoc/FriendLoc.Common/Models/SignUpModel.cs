using System;
using FriendLoc.Entity;

namespace FriendLoc.Model
{
    public class SignUpModel
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string FullName { get; set; }
        public string AvtImgPath { get; set; }
        
        public Gender Gender { get; set; }
    }
}
