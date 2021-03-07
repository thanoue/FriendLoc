
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
using AndroidX.AppCompat.App;
using Google.Android.Material.Button;

namespace FriendLoc.Droid.Activities
{
    [Activity(MainLauncher = true)]
    public class LoginActivity : BaseActivity
    {
        protected override int LayoutResId => Resource.Layout.activity_login;
        protected override bool IsFullScreen => true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentNavigation);
            Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);

            Window.SetStatusBarColor(this.Resources.GetColor(Resource.Color.colorScreenBackground));
            Window.SetNavigationBarColor(this.Resources.GetColor(Resource.Color.colorScreenBackground));

            var flag = (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LightStatusBar;
            Window.DecorView.SystemUiVisibility = flag;

            FindViewById<View>(Resource.Id.extended_fab).Click += delegate
            {
                StartActivity(typeof(SignUpActivity));
            };

            FindViewById<MaterialButton>(Resource.Id.loginBtn).Click += delegate
            {
                StartActivity(typeof(HomeActivity));
            };
            // Create your application here
        }
    }
}
