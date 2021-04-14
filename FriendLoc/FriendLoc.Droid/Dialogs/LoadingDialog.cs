﻿using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Com.Airbnb.Lottie;

namespace FriendLoc.Droid.Dialogs
{
    public interface ILoadingDialog
    {
        void Dismiss();
        string TAG { get; }
    }

    public class LoadingDialog : DialogFragment, ILoadingDialog
    {
        public string TAG => "LoadingDialog";

        LottieAnimationView _animView;

        public LoadingDialog()
        {
            SetStyle( DialogFragment.StyleNoFrame,Resource.Style.ShapeAppearanceOverlay_MaterialComponents_MaterialCalendar_Window_Fullscreen);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            var view = inflater.Inflate(Resource.Layout.dialog_loadin, container);

            _animView = view.FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            _animView.SetScaleType(ImageView.ScaleType.FitXy);
            _animView.Scale = 10;
            _animView.Speed = 2f;
            _animView.PlayAnimation();

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }
    }

}
