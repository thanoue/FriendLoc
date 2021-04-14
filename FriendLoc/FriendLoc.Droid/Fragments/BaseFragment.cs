
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
using FriendLoc.Common;
using FriendLoc.Common.Services;
using Google.Android.Material.AppBar;
using FriendLoc.Droid.Activities;
using Plugin.CurrentActivity;

namespace FriendLoc.Droid.Fragments
{
    public abstract class BaseFragment : Fragment
    {
        public IGlobalUIService UtilUI => ServiceLocator.Instance.Get<IGlobalUIService>();
        public BaseActivity CurrentActivity => (BaseActivity)CrossCurrentActivity.Current.Activity;

    }
}
