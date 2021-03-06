
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

namespace FriendLoc.Droid.Activities
{
    [Activity(MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_login);

            FindViewById<View>(Resource.Id.extended_fab).Click += delegate
            {
                StartActivity(typeof(HomeActivity));
            };
            // Create your application here
        }
    }
}
