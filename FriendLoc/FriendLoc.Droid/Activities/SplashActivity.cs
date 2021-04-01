
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
using FriendLoc.Entity;

namespace FriendLoc.Droid.Activities
{
    [Activity(MainLauncher = true)]
    public class SplashActivity : BaseActivity
    {
        protected override int LayoutResId => Resource.Layout.activity_splash;
        protected override bool IsFullScreen => true;
        protected override int ThemeResId => Resource.Style.AppTheme;

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
                CheckPermission(PermissionCallback, Android.Manifest.Permission.AccessFineLocation,
                        Android.Manifest.Permission.AccessCoarseLocation,
                        Android.Manifest.Permission.ControlLocationUpdates,
                        Android.Manifest.Permission.Camera,
                        Android.Manifest.Permission.ReadExternalStorage,
                        Android.Manifest.Permission.WriteExternalStorage);
            }
        }

        void PermissionCallback()
        {
            var loggedInUser = ServiceInstances.SecureStorage.GetObject<User>(Constants.LoggedinUser);
            var userToken = ServiceInstances.SecureStorage.Fetch(Constants.UserToken);

            if (!string.IsNullOrEmpty(userToken))
            {
                DateTime lastestLoggedIn;

                if (DateTime.TryParse(ServiceInstances.SecureStorage.Fetch(Constants.LastestLoggedIn), out lastestLoggedIn))
                {
                    var period = DateTime.Now - lastestLoggedIn;

                    if (period.TotalMinutes > 50)
                    {
                        ServiceInstances.SecureStorage.DeleteObject(Constants.LoggedinUser);
                        ServiceInstances.SecureStorage.DeleteObject(Constants.UserToken);

                        StartNewActivity(typeof(LoginActivity));
                        return;
                    }

                    ServiceInstances.ResourceService.UserToken = userToken;
                    UserSession.Instance.LoggedinUser = loggedInUser;

                    StartNewActivity(typeof(HomeActivity));
                }
                else
                    StartNewActivity(typeof(LoginActivity));
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
