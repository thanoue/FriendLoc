using System;
namespace FriendLoc.Common.Models
{
    public class User : BaseEntity
    {
        public string LoginName { get; set; }
        public string AvtUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }

        public User()
        {
        }
    }
}
