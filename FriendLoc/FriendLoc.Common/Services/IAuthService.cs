using System;
using System.Threading.Tasks;
using FriendLoc.Entity;
using FriendLoc.Model;

namespace FriendLoc.Common.Services
{
    public interface IAuthService
    {
        void SignUp(SignUpModel model, Action<string> errorCallback, Action successCallback);
        Task<string> Login(string loginName, string password, Action<string> errorCallback);
        Task<string> PushImageToServer(string path, Action<int> progressAction,string folderName);
    }
}
