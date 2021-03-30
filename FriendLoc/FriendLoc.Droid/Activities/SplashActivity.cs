
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FriendLoc.Common;

namespace FriendLoc.Droid.Activities
{
    [Activity(MainLauncher = true)]
    public class SplashActivity : BaseActivity
    {
        protected override int LayoutResId => Resource.Layout.activity_splash;
        protected override bool IsFullScreen => true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                CheckPermission(PermissionCallback, Android.Manifest.Permission.AccessBackgroundLocation,
                                 Android.Manifest.Permission.AccessFineLocation,
                                 Android.Manifest.Permission.AccessCoarseLocation,
                                 Android.Manifest.Permission.ControlLocationUpdates,
                                 Android.Manifest.Permission.Camera,
                                 Android.Manifest.Permission.ReadExternalStorage,
                                 Android.Manifest.Permission.WriteExternalStorage
                                );
            }
            else
            {
                CheckPermission(PermissionCallback,Android.Manifest.Permission.AccessFineLocation,
                        Android.Manifest.Permission.AccessCoarseLocation,
                        Android.Manifest.Permission.ControlLocationUpdates,
                        Android.Manifest.Permission.Camera,
                        Android.Manifest.Permission.ReadExternalStorage,
                        Android.Manifest.Permission.WriteExternalStorage);
            }
        }

        void PermissionCallback()
        {
            var loginName = ServiceInstances.SecureStorage.Fetch(Constants.LoginName);
            var password = ServiceInstances.SecureStorage.Fetch(Constants.Password);

            if (!string.IsNullOrEmpty(loginName))
            {
                StartLoading();

                ServiceInstances.AuthService.Login(loginName, password, (err) => {

                    StopLoading();
                    ErrorToast(err);

                    StartNewActivity(typeof(LoginActivity));

                }, () => {

                    StopLoading();

                    StartNewActivity(typeof(HomeActivity));

                });
            }
            else
            {
                StartNewActivity(typeof(LoginActivity));
            }
        }

        void StartNewActivity(Type type)
        {
            var intent = new Intent(this, type);

            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.ClearTop | ActivityFlags.NewTask);

            StartActivity(intent);
        }
    }
}
