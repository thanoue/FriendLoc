using System;
using Android.App;
using Android.Graphics;
using AndroidX.AppCompat.App;
using FriendLoc.Common.Services;
using FriendLoc.Droid.Dialogs;
using Google.Android.Material.Snackbar;
using Plugin.CurrentActivity;

namespace FriendLoc.Droid.Services
{
    public class DroidGlobalUIService : IGlobalUIService
    {
        public DroidGlobalUIService()
        {
        }

        private ILoadingDialog _loadingDialog;
        private int _loadingCount = 0;

        public void StartLoading()
        {
            _loadingCount += 1;

            if (_loadingCount > 1)
                return;

            CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
            {
                if (CrossCurrentActivity.Current.Activity is AppCompatActivity)
                {
                    _loadingDialog = new LoadingDialog();
                    ((LoadingDialog) _loadingDialog).Show(
                        ((AppCompatActivity) CrossCurrentActivity.Current.Activity).SupportFragmentManager,
                        _loadingDialog.TAG);
                }
                else
                {
                    _loadingDialog = new LoadingDialogOld();
                    ((LoadingDialogOld) _loadingDialog).Show(((AppCompatActivity)CrossCurrentActivity.Current.Activity).SupportFragmentManager,
                        _loadingDialog.TAG);
                }
            });
        }

        public void StopLoading()
        {
            _loadingCount -= 1;
                
            if (_loadingCount <= 0)
            {
                CrossCurrentActivity.Current.Activity.RunOnUiThread(() => { _loadingDialog.Dismiss(); });
                _loadingCount = 0;
            }
        }

        public void ErrorToast(string content, Action onClick = null)
        {
            ToastMessage(content, Resource.Color.colorError, 5000, onClick);
        }

        public void WarningToast(string content, Action onClick = null)
        {
            ToastMessage(content, Resource.Color.colorWarning, Snackbar.LengthShort, onClick);
        }

        public void InfToast(string content, Action onClick = null)
        {
            ToastMessage(content, Resource.Color.colorInfo, Snackbar.LengthShort, onClick);
        }

        public void SuccessToast(string content, Action onClick = null)
        {
            ToastMessage(content, Resource.Color.colorSuccess, Snackbar.LengthShort, onClick);
        }

        private void ToastMessage(string content, int backgroundColorResId, int duration, Action onClick = null)
        {
            CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
            {
                var bar = Snackbar.Make(CrossCurrentActivity.Current.Activity.Window.DecorView, content, duration)
                    .SetBackgroundTint(backgroundColorResId)
                    .SetBackgroundTintMode(PorterDuff.Mode.Lighten)
                    .SetTextColor(Color.White)
                    .SetAction("Dismiss", (view) => { onClick?.Invoke(); });

                bar.SetAnimationMode(Snackbar.AnimationModeSlide);

                bar.Show();
            });
        }
    }
}