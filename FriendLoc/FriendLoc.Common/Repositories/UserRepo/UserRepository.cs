using System;
using FriendLoc.Entity;

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
