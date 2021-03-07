
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
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.AppBar;
using Google.Android.Material.Navigation;

namespace FriendLoc.Droid.Activities
{
    [Activity(Theme = "@style/AppThemeTranslucent")]
    public class HomeActivity : BaseActivity
    {
        MaterialToolbar _toolBar;
        DrawerLayout _drawerLayout;

        protected override int LayoutResId => Resource.Layout.activity_home;
        protected override bool IsFullScreen => true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _toolBar = FindViewById<MaterialToolbar>(Resource.Id.topAppBar);
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.container);

            _toolBar.Click += delegate
            {
                _drawerLayout.Open();
            };
        }
    }
}
