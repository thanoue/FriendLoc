
using System;
using FriendLoc.Common.Services;

namespace FriendLoc.Common
{
    public static class ServiceInstances
    {
        public static IFileService FileService => ServiceLocator.Instance.Get<IFileService>();
        public static ILoggerService LoggerService => ServiceLocator.Instance.Get<ILoggerService>();
    }
}
