
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using Com.Airbnb.Lottie;
using FriendLoc.Common;
using Google.Android.Material.AppBar;
using Google.Android.Material.Dialog;
using Google.Android.Material.Snackbar;
using Google.Android.Material.TextView;

namespace FriendLoc.Droid.Activities
{
    public abstract class BaseActivity : AppCompatActivity, View.IOnClickListener
    {
        const int REQUEST_PERMISSIONS = 11;

        protected abstract int LayoutResId { get; }
        protected virtual bool IsFullScreen => false;
        protected virtual string HeaderTitle => "";
        protected virtual int NavigationIconResId => Resource.Drawable.ic_back_24;
        protected virtual bool IsConfirmBeforeBack => false;

        RelativeLayout _loadingView;
        LottieAnimationView _animView;
        Action _permissionCallback;
        MaterialToolbar _toolBar;
        RelativeLayout _rootView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_root);

            _rootView = FindViewById<RelativeLayout>(Resource.Id.baseContentView);

            _loadingView = FindViewById<RelativeLayout>(Resource.Id.loadingArea);
            _animView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            _animView.SetScaleType(ImageView.ScaleType.FitXy);
            _animView.Scale = 10;
            _animView.Speed = 2f;

            if (IsFullScreen)
            {
                LayoutInflater.Inflate(LayoutResId, _rootView, true);
            }
            else
            {
                LayoutInflater.Inflate(Resource.Layout.activity_base, _rootView, true);

                LayoutInflater.Inflate(LayoutResId, FindViewById<RelativeLayout>(Resource.Id.contentView), true);

                _toolBar = FindViewById<MaterialToolbar>(Resource.Id.topAppBar);

                _toolBar.SetNavigationIcon(NavigationIconResId);
                _toolBar.SetNavigationOnClickListener(this);
                _toolBar.Title = HeaderTitle;
            };
        }

        protected void ErrorToast(string content, Action onClick = null)
        {
            ToastMessage(content, Resource.Color.colorError,5000, onClick);
        }

        protected void WarningToast(string content, Action onClick = null)
        {
            ToastMessage(content, Resource.Color.colorWarning, Snackbar.LengthShort, onClick);
        }

        protected void InfToast(string content, Action onClick = null)
        {
            ToastMessage(content, Resource.Color.colorInfo, Snackbar.LengthShort, onClick);
        }

        protected void SuccessToast(string content, Action onClick = null)
        {
            ToastMessage(content, Resource.Color.colorSuccess, Snackbar.LengthShort, onClick);
        }

        private void ToastMessage(string content, int backgroundColorResId, int duration, Action onClick = null)
        {
            RunOnUiThread(() =>
            {
                var bar = Snackbar.Make(_rootView, content, duration)
                       .SetBackgroundTint(backgroundColorResId)
                       .SetBackgroundTintMode(PorterDuff.Mode.Darken)
                       .SetTextColor(Color.White)
                       .SetAction("Dismiss", (view) =>
                       {
                           onClick?.Invoke();
                       });

                bar.SetAnimationMode(Snackbar.AnimationModeSlide);

                bar.Show();
            });
        }

        protected void StartLoading(string loadingContent = "Loading...")
        {
            RunOnUiThread(() =>
            {
                Window.SetFlags(WindowManagerFlags.NotTouchable, WindowManagerFlags.NotTouchable);
                _loadingView.Visibility = ViewStates.Visible;
                _animView.PlayAnimation();
            });
        }

        protected void StopLoading()
        {
            RunOnUiThread(() =>
            {
                Window.ClearFlags(WindowManagerFlags.NotTouchable);
                _loadingView.Visibility = ViewStates.Gone;
                _animView.CancelAnimation();
            });
        }

        protected virtual void OnSaveRequest()
        {
            this.Finish();
        }

        protected virtual void OnCancel()
        {
            this.Finish();
        }

        void OnBackClick()
        {
            if (!IsConfirmBeforeBack)
            {
                Finish();
                return;
            }

            var builder = new MaterialAlertDialogBuilder(this)
                                .SetTitle("Alert")
                                .SetMessage("Do you want to save it first?")
                                .SetNeutralButton("Cancel", (sender, e) =>
                                {

                                })
                                .SetNegativeButton("No", (sender, e) =>
                                {
                                    OnCancel();
                                })
                                .SetPositiveButton("Yes", (sender, e) =>
                                {
                                    OnSaveRequest();
                                });


            builder.Show();
        }

        public override void OnBackPressed()
        {
            if (_loadingView.Visibility == ViewStates.Visible)
            {
                return;
            }

            OnBackClick();
        }

        public void OnClick(View v)
        {
            OnBackClick();
        }

        bool IsGranted(params string[] pers)
        {
            foreach (var per in pers)
            {
                if (ActivityCompat.CheckSelfPermission(this, per) != Permission.Granted)
                {
                    return false;
                }
            }

            return true;
        }

        protected void CheckPermission(Action successCallback, params string[] pers)
        {
            _permissionCallback = successCallback;
            CheckPermission(pers);
        }

        protected void CheckPermission(params string[] pers)
        {
            if (!IsGranted(pers))
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, pers[0]))
                {
                    ActivityCompat.RequestPermissions(this, pers,
                      REQUEST_PERMISSIONS);
                }
                else
                {
                    ActivityCompat.RequestPermissions(this, pers,
                      REQUEST_PERMISSIONS);
                }
            }
            else
            {
                _permissionCallback?.Invoke();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode != REQUEST_PERMISSIONS)
                return;

            if (grantResults.Length > 0 && grantResults[0] == Permission.Denied)
            {
                Toast.MakeText(ApplicationContext, "Please allow the permission", ToastLength.Long).Show();
            }
            else
            {
                _permissionCallback?.Invoke();
            }
        }
    }
}
