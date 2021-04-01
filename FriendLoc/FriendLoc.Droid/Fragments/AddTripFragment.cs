
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;

namespace FriendLoc.Droid.Fragments
{
    public class AddTripFragment : BaseFragment
    {
        public override int ResId => Resource.Layout.fragment_add_trip;
        public override string HeaderTitle => "Add new Trip";
        public override bool IsAskBeforeDismiss => true;

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }
    }
}
