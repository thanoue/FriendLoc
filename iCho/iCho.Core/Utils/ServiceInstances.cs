using System;
using iCho.Service;

namespace iCho.Core.Utils
{
    public static class ServiceInstances
    {
        public static  IApiClient ApiClient => ServiceLocator.Instance.Get<IApiClient>();
        public static ISocialMediaAuth SocialMediaAuthService => ServiceLocator.Instance.Get<ISocialMediaAuth>();
    }
}
