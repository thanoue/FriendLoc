using System;
using Android.App;
using Android.Runtime;
using FriendLoc.Common;
using FriendLoc.Common.Repositories;
using FriendLoc.Common.Services;
using FriendLoc.Common.Services.Impl;
using FriendLoc.Droid.Services;
using Plugin.CurrentActivity;

namespace FriendLoc.Droid
{
    [Application(UsesCleartextTraffic = true)]
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
            ServiceLocator.Instance.Register<IUserRepository, UserRepository>();
            ServiceLocator.Instance.Register<ITripLocationRepository, TripLocationRepository>();
            ServiceLocator.Instance.Register<IUserMilestoneRepository, UserMilestoneRepository>();
            
            ServiceLocator.Instance.Register<INativeTrigger, DroidNativeTrigger>();
            ServiceLocator.Instance.Register<IResourceService, DroidResourceService>(this.ApplicationContext);
            ServiceLocator.Instance.Register<IAuthService, FirebaseAuthService>();
            ServiceLocator.Instance.Register<ISecureStorageService, DroidSecureStorageService>(this);
            ServiceLocator.Instance.Register<IQRCodeService, QRCodeService>(this.ApplicationContext);

        }
    }
}
