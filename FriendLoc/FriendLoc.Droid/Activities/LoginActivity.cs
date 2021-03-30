
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
using FriendLoc.Common;
using FriendLoc.Controls;
using Google.Android.Material.Button;

namespace FriendLoc.Droid.Activities
{
    [Activity]
    public class LoginActivity : BaseActivity
    {
        protected override int LayoutResId => Resource.Layout.activity_login;
        protected override bool IsFullScreen => true;

        CheckBox _keepLoggedinCheckBox;
        CustomEditText _loginNameTxt, _passwordTxt;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentNavigation);
            Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);

            Window.SetStatusBarColor(this.Resources.GetColor(Resource.Color.colorScreenBackground));
            Window.SetNavigationBarColor(this.Resources.GetColor(Resource.Color.colorScreenBackground));

            _loginNameTxt = FindViewById<CustomEditText>(Resource.Id.userNameTxt);
            _passwordTxt = FindViewById<CustomEditText>(Resource.Id.passwordTxt);
            _keepLoggedinCheckBox = FindViewById<CheckBox>(Resource.Id.keepLoggedinCheckBox);

            var flag = (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LightStatusBar;
            Window.DecorView.SystemUiVisibility = flag;

            FindViewById<View>(Resource.Id.extended_fab).Click += delegate
            {
                StartActivity(typeof(SignUpActivity));
            };

            FindViewById<MaterialButton>(Resource.Id.loginBtn).Click += delegate
            {
                Login();
            };
            // Create your application here
        }

        bool ValidateFields()
        {
            var res = true;

            if (string.IsNullOrEmpty(_loginNameTxt.Text) || _loginNameTxt.Text.Length < ServiceInstances.ResourceService.LoginNameMinLength)
            {
                _loginNameTxt.Error = "Login Name min length is " + ServiceInstances.ResourceService.LoginNameMinLength.ToString();
                res = false;
            }

            if (_loginNameTxt.Text.Length > ServiceInstances.ResourceService.LoginNameMaxLength)
            {
                _loginNameTxt.Error = ("Login Name max length is " + ServiceInstances.ResourceService.LoginNameMaxLength);
                res = false;
            }

            if (string.IsNullOrEmpty(_passwordTxt.Text) || _passwordTxt.Text.Length < ServiceInstances.ResourceService.PasswordMinLength)
            {
                _passwordTxt.Error = "Password min length is " + ServiceInstances.ResourceService.PasswordMinLength.ToString();
                res = false;
            }

            if (_passwordTxt.Text.Length > ServiceInstances.ResourceService.PasswordMaxLength)
            {
                _passwordTxt.Error = ("Password max length is " + ServiceInstances.ResourceService.PasswordMaxLength);
                res = false;
            }
            
            return res;
        }

        void Login()
        {
            if (!ValidateFields())
                return;

            StartLoading();

            ServiceInstances.AuthService.Login(_loginNameTxt.Text, _passwordTxt.Text,(err)=> {

                StopLoading();
                ErrorToast(err);

            },()=> {

                StopLoading();
                StartActivity(typeof(HomeActivity));

                if (_keepLoggedinCheckBox.Checked)
                {
                    ServiceInstances.SecureStorage.Store(Constants.LoginName,_loginNameTxt.Text);
                    ServiceInstances.SecureStorage.Store(Constants.Password, _passwordTxt.Text);
                }
                else
                {
                    ServiceInstances.SecureStorage.DeleteObject(Constants.LoginName);
                    ServiceInstances.SecureStorage.DeleteObject(Constants.Password);
                }
            });
        }
    }
}
