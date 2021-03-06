
using System;
using FriendLoc.Common.Repositories;
using FriendLoc.Common.Services;

namespace FriendLoc.Common
{
    public static class ServiceInstances
    {
        public static IFileService FileService => ServiceLocator.Instance.Get<IFileService>();
        public static ILoggerService LoggerService => ServiceLocator.Instance.Get<ILoggerService>();
        public static ITripRepository TripRepository => ServiceLocator.Instance.Get<ITripRepository>();
        public static IUserRepository UserRepository => ServiceLocator.Instance.Get<IUserRepository>();
        public static INativeTrigger NativeTrigger => ServiceLocator.Instance.Get<INativeTrigger>();
    }
}
