using System;
using Android.App;
using Android.Runtime;
using FriendLoc.Common;
using FriendLoc.Common.Repositories;
using FriendLoc.Common.Services;
using FriendLoc.Droid.Services;
using Plugin.CurrentActivity;

namespace FriendLoc.Droid
{
    [Application]
    public class MainApp : Application
    {
        protected MainApp(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            CrossCurrentActivity.Current.Init(this);
            Xamarin.Essentials.Platform.Init(this);

            ServiceLocator.Instance.Register<IFileService, DroidFileService>();
            ServiceLocator.Instance.Register<ILoggerService, LoggerService>();
            ServiceLocator.Instance.Register<ITripRepository, TripRepository>();
            ServiceLocator.Instance.Register<IUserRepository, UserRepository>();
        }
    }
}
