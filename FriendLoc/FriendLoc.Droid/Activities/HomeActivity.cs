
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
using BumpTech.GlideLib;
using FriendLoc.Common;
using FriendLoc.Droid.Fragments;
using Google.Android.Material.AppBar;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.ImageView;
using Google.Android.Material.Navigation;

namespace FriendLoc.Droid.Activities
{
    [Activity]
    public class HomeActivity : BaseActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        MaterialToolbar _toolBar;
        DrawerLayout _drawerLayout;
        NavigationView _navigationView;
        ShapeableImageView _avtImmg;
        TextView _fullNameTv, _phoneNumberTv;
        FrameLayout _fragmentContainer;

        protected override int LayoutResId => Resource.Layout.activity_home;
        protected override bool IsFullScreen => true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _toolBar = FindViewById<MaterialToolbar>(Resource.Id.topAppBar);
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.container);
            _navigationView = FindViewById<NavigationView>(Resource.Id.navigationView);
            _fragmentContainer = FindViewById<FrameLayout>(Resource.Id.homeFragmentContainer);

            var header = _navigationView.GetHeaderView(0);

            _avtImmg = header.FindViewById<ShapeableImageView>(Resource.Id.avtImg);
            _fullNameTv = header.FindViewById<TextView>(Resource.Id.nameTv);
            _phoneNumberTv = header.FindViewById<TextView>(Resource.Id.phoneNumberTv);

            _toolBar.Click += delegate
            {
                _drawerLayout.Open();
            };

            _navigationView.SetNavigationItemSelectedListener(this);


            Glide.With(this).Load(UserSession.Instance.LoggedinUser.AvtUrl).Into(_avtImmg);
            _fullNameTv.Text = UserSession.Instance.LoggedinUser.FullName;
            _phoneNumberTv.Text = UserSession.Instance.LoggedinUser.PhoneNumber;
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            menuItem.SetChecked(true);

            _drawerLayout.Close();

            switch (menuItem.ItemId)
            {
                case Resource.Id.logoutItem:

                    ServiceInstances.SecureStorage.DeleteObject(Constants.LoggedinUser);
                    ServiceInstances.SecureStorage.DeleteObject(Constants.UserToken);

                    var intent = new Intent(this, typeof(LoginActivity));

                    intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.ClearTop | ActivityFlags.NewTask);

                    StartActivity(intent);

                    break;

                case Resource.Id.tripItem:

                    ServiceInstances.TripRepository.GetByJoinedUser(UserSession.Instance.LoggedinUser.Id).ContinueWith((res) =>
                    {
                        var trips = res.Result;

                        var fragment = new TripsFragment(trips);

                        var ft = SupportFragmentManager.BeginTransaction();

                        ft.SetCustomAnimations(Resource.Animation.design_bottom_sheet_slide_in, Resource.Animation.design_bottom_sheet_slide_out);

                        ft.Replace(_fragmentContainer.Id, fragment).Commit();
                    });

                    break;
            }

            return true;
        }
    }
}
