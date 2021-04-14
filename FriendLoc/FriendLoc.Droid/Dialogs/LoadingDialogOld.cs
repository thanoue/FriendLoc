using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;

namespace FriendLoc.Droid.Dialogs
{
    public class LoadingDialogOld : DialogFragment,ILoadingDialog
    {
        public  string TAG => "LoadingDialogOld";

        LottieAnimationView _animView;

        public LoadingDialogOld()
        {
            SetStyle( DialogFragmentStyle.NoFrame, Resource.Style.ShapeAppearanceOverlay_MaterialComponents_MaterialCalendar_Window_Fullscreen);
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
    }
}
