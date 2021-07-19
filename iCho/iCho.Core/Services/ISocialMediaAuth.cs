using System;
using System.Threading.Tasks;
using iCho.Core.Utils;

namespace iCho.Service
{
    public interface ISocialMediaAuth
    {
        void Init();
        string PlatformType { get; }
        SocialMediaType AuthType { get; }
        Task<string> GoogleLogin();
        Task<string> FacebookLogin();
    }
}
