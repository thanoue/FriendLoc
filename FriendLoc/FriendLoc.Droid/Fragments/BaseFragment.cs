
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.Fragment.App;
using Android.Widget;
using Google.Android.Material.AppBar;
using FriendLoc.Droid.Activities;
using Plugin.CurrentActivity;

namespace FriendLoc.Droid.Fragments
{
    public abstract class BaseFragment : Fragment, AndroidX.AppCompat.Widget.Toolbar.IOnMenuItemClickListener
    {
        public virtual int ResId => 0;
        public virtual string HeaderTitle => "";
        public virtual bool IsAskBeforeDismiss => false;

        MaterialToolbar _appBar;

        public BaseActivity CurrentActivity => (BaseActivity)CrossCurrentActivity.Current.Activity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var baseView = inflater.Inflate(Resource.Layout.fragment_base, container,false);

            inflater.Inflate(ResId, baseView.FindViewById<RelativeLayout>(Resource.Id.contentView),true);

            _appBar = baseView.FindViewById<MaterialToolbar>(Resource.Id.topAppBar);

            _appBar.SetOnMenuItemClickListener(this);

            _appBar.Title = HeaderTitle;

            return baseView;
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.closeIcon:

                    CurrentActivity.OnBackClick();

                    break;
            }
            return true;
        }
    }
}
