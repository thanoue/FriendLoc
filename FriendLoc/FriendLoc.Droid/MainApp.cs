using System;
using Android.App;
using Android.Runtime;
using FriendLoc.Common;
using FriendLoc.Common.Services;
using FriendLoc.Droid.Services;

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

            Xamarin.Essentials.Platform.Init(this);

            ServiceLocator.Instance.Register<IFileService, DroidFileService>();

            ServiceLocator.Instance.Register<ILoggerService, LoggerService>();
        }
    }
}
