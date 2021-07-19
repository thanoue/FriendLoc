using System;
using System.IO;
using System.Threading.Tasks;
using iCho.Core.ApiModel;

namespace iCho.Service
{
    public interface IApiClient
    {
        Task<UserApiModel> Signup(UserApiModel model, byte[] avtData = null);
        Task<UserApiModel> UpdateUserLoginInfo(UserApiModel model, byte[] avtData = null);
        Task<HouseApiModel> CreateHouse(HouseApiModel model, byte[] avtData = null);
        Task<UserLoginApiModel> LoginByFacebook(string token);
        Task<UserLoginApiModel> LoginByLoginName(string loginName,string password);
        Task<UserLoginApiModel> LoginByGoogle(string token);
        UserLoginApiModel User { get; }
    }
}
