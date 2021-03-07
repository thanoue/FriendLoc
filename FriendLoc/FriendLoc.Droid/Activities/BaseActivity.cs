
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using FriendLoc.Common;
using Google.Android.Material.AppBar;
using Google.Android.Material.Dialog;

namespace FriendLoc.Droid.Activities
{
    public abstract class BaseActivity : AppCompatActivity,View.IOnClickListener
    {
        const int REQUEST_PERMISSIONS = 1;

        protected abstract int LayoutResId { get; }
        protected virtual bool IsFullScreen => false;
        protected virtual string HeaderTitle => "";
        protected virtual int NavigationIconResId => Resource.Drawable.ic_back_24;
        protected virtual bool IsConfirmBeforeBack => false;

        Action _permissionCallback;
        MaterialToolbar _toolBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_root);
            var rootContentView = FindViewById<RelativeLayout>(Resource.Id.baseContentView);

            if (IsFullScreen)
            {
                LayoutInflater.Inflate(LayoutResId, rootContentView, true);
            }
            else
            {
                LayoutInflater.Inflate(Resource.Layout.activity_base, rootContentView, true);

                LayoutInflater.Inflate(LayoutResId, FindViewById<RelativeLayout>(Resource.Id.contentView), true);

                _toolBar = FindViewById<MaterialToolbar>(Resource.Id.topAppBar);

                _toolBar.SetNavigationIcon(NavigationIconResId);
                _toolBar.SetNavigationOnClickListener(this);
                _toolBar.Title = HeaderTitle;
            };
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

        protected void CheckPermision(Action successCallback, params string[] pers)
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
