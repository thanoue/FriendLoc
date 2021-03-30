using System;
using System.Threading.Tasks;
using Firebase.Database;

namespace FriendLoc.Common.Services.Impl
{
    public abstract class ResourceService : IResourceService
    {

        public ResourceService()
        {

        }

        private string _useToken = "";
        public string UserToken
        {
            get
            {
                return _useToken;
            }
            set
            {
                _useToken = value;
            }
        }

        public abstract int LoginNameMinLength { get; }

        public abstract int LoginNameMaxLength { get; }

        public abstract int PasswordMinLength { get; }

        public abstract int PasswordMaxLength { get; }

    }
}
