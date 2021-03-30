using System;
using System.Threading.Tasks;
using FriendLoc.Entity;
using FriendLoc.Model;

namespace FriendLoc.Common.Services
{
    public interface IAuthService
    {
        void SignUp(SignUpModel model, Action<string> errorCallback, Action successCallback);
        Task Login(string loginName, string password,Action<string> errorCallback, Action successCallback);
        Task<string> PushUserAvatar(string path, Action<int> progressAction);
    }
}
