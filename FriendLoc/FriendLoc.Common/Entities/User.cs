using System;
namespace FriendLoc.Entity
{
    public class User : BaseEntity
    {
        public string LoginName { get; set; }
        public string AvtUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string CountryCode { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }

        public User()
        {

        }
    }

    public enum Gender
    {
        Male,
        Female,
        Others
    }
}
