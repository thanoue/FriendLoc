using System;
using FriendLoc.Common.Models;

namespace FriendLoc.Common.Repositories
{
    public class UserRepository : BaseRepo<User> , IUserRepository
    {
        public override string Path => "Users";

        public UserRepository()
        {
        }
    }
}
