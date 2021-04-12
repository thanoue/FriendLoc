
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
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
using Com.Nguyenhoanglam.Imagepicker.Model;
using Com.Nguyenhoanglam.Imagepicker.UI.Imagepicker;
using FriendLoc.Common;
using FriendLoc.Droid.Fragments;
using Google.Android.Material.AppBar;
using Google.Android.Material.Dialog;
using Google.Android.Material.Snackbar;
using Google.Android.Material.TextView;
using Fragment = AndroidX.Fragment.App.Fragment;
namespace FriendLoc.Droid.Activities
{
    public interface IImgSelectionObj
    {
        public void OnImgSelected(string path,Activity activity);
    }

    public abstract class BaseActivity : AppCompatActivity, View.IOnClickListener
    {
        const int REQUEST_PERMISSIONS = 11;

        protected abstract int LayoutResId { get; }
        protected virtual bool IsFullScreen => false;
        protected virtual string HeaderTitle => "";
        protected virtual int NavigationIconResId => Resource.Drawable.ic_back_24;
        protected virtual bool IsAskBeforeDismiss => false;
        protected virtual int ThemeResId => Resource.Style.WhiteNavigtionBaseTheme;

        RelativeLayout _loadingView;
        LottieAnimationView _animView;
        Action _permissionCallback;
        MaterialToolbar _toolBar;
        RelativeLayout _rootView;
        FrameLayout _fragmentContainer;
        BaseFragment _currentFragment;

        IImgSelectionObj _imgSelectionObj;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(ThemeResId);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_root);

            _rootView = FindViewById<RelativeLayout>(Resource.Id.baseContentView);
            _fragmentContainer = FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
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

        public void StartLoading(string loadingContent = "Loading...")
        {
            RunOnUiThread(() =>
            {
                Window.SetFlags(WindowManagerFlags.NotTouchable, WindowManagerFlags.NotTouchable);
                _loadingView.Visibility = ViewStates.Visible;
                _animView.PlayAnimation();
            });
        }

        public void StopLoading()
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

        public virtual void OnCancel()
        {
            if (_currentFragment != null)
            {
                RemoveFragment(_currentFragment);
                return;
            }

            this.Finish();
        }

        public void SetImgSelectionListner(IImgSelectionObj obj)
        {
            _imgSelectionObj = obj;
        }

        public void SelectImageFromGallery()
        {
            CheckPermission(() =>
            {
                ServiceInstances.FileService.SetRootFolderPath(ServiceInstances.FileService.GetSdCardFolder());

                ImagePicker.With(this)
                   .SetFolderMode(true)
                   .SetCameraOnly(false)
                   .SetFolderTitle("Album")
                   .SetMultipleMode(false)
                   .SetMaxSize(1)
                   .SetBackgroundColor("#ffffff")
                   .SetAlwaysShowDoneButton(false)
                   .SetKeepScreenOn(true)
                   .SetToolbarColorResId(Resource.Color.colorPrimary)
                   .SetStatusBarColor("#431DCC")
                   .Start();

            }, Manifest.Permission.Camera, Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode != Result.Ok)
                return;

            if (Config.RcPickImages == requestCode)
            {
                var urls = data.GetParcelableArrayListExtra(Config.ExtraImages).Cast<Image>().ToList();

                if (urls != null && urls.Count > 0)
                {
                    _imgSelectionObj?.OnImgSelected(urls[0].Path,this);
                }
            }
        }

        public void OnBackClick()
        {
            var builder = new MaterialAlertDialogBuilder(this)
                                .SetTitle("Alert")
                                .SetMessage("Do you really want to discard all your changes?")
                                .SetNeutralButton("Cancel", (sender, e) =>
                                {

                                })
                                .SetNegativeButton("No", (sender, e) =>
                                {
                                })
                                .SetPositiveButton("Yes", (sender, e) =>
                                {
                                    OnCancel();
                                });

            if (_currentFragment != null)
            {
                if (_currentFragment.IsAskBeforeDismiss)
                {
                    builder.Show();
                    return;
                }
                else
                {
                    OnCancel();
                    return;
                }
            }

            if (!IsAskBeforeDismiss)
            {
                OnCancel();
                return;
            }

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

        public void LoadFragment(BaseFragment fragment, bool isWithAnim = true)
        {
            var ft = SupportFragmentManager.BeginTransaction();

            if (isWithAnim)
                ft.SetCustomAnimations(Resource.Animation.design_bottom_sheet_slide_in, Resource.Animation.design_bottom_sheet_slide_out);

            ft.Replace(_fragmentContainer.Id, fragment).Commit();

            _currentFragment = fragment;
        }

        public void RemoveFragment(BaseFragment fragment, bool isWithAnim = true)
        {
            var ft = SupportFragmentManager.BeginTransaction();

            if (isWithAnim)
                ft.SetCustomAnimations(Resource.Animation.design_bottom_sheet_slide_in, Resource.Animation.design_bottom_sheet_slide_out);

            ft.Remove(fragment).Commit();

            _currentFragment = null;
        }

        public void CheckPermission(Action successCallback, params string[] pers)
        {
            _permissionCallback = successCallback;
            CheckPermission(pers);
        }

        public void CheckPermission(params string[] pers)
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
