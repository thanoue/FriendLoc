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
using FriendLoc.Droid.Fragments;
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

            Window.SetStatusBarColor(this.Resources.GetColor(Resource.Color.colorScreenBackground));

            _loginNameTxt = FindViewById<CustomEditText>(Resource.Id.userNameTxt);
            _passwordTxt = FindViewById<CustomEditText>(Resource.Id.passwordTxt);
            _keepLoggedinCheckBox = FindViewById<CheckBox>(Resource.Id.keepLoggedinCheckBox);

            var flag = (Android.Views.StatusBarVisibility) Android.Views.SystemUiFlags.LightStatusBar;
            Window.DecorView.SystemUiVisibility = flag;

            FindViewById<View>(Resource.Id.extended_fab).Click += delegate { StartActivity(typeof(SignUpActivity)); };

            FindViewById<MaterialButton>(Resource.Id.loginBtn).Click += delegate
            {
                //this.LoadFragment(new AddTripFragment());
                LoginAsync();
            };

            _loginNameTxt.Text = "testuser";
            _passwordTxt.Text = "123456789";
            // Create your application here
        }

        bool ValidateFields()
        {
            var res = true;

            if (string.IsNullOrEmpty(_loginNameTxt.Text) ||
                _loginNameTxt.Text.Length < ServiceInstances.ResourceService.LoginNameMinLength)
            {
                _loginNameTxt.Error = "Login Name min length is " +
                                      ServiceInstances.ResourceService.LoginNameMinLength.ToString();
                res = false;
            }

            if (_loginNameTxt.Text.Length > ServiceInstances.ResourceService.LoginNameMaxLength)
            {
                _loginNameTxt.Error =
                    ("Login Name max length is " + ServiceInstances.ResourceService.LoginNameMaxLength);
                res = false;
            }

            if (string.IsNullOrEmpty(_passwordTxt.Text) ||
                _passwordTxt.Text.Length < ServiceInstances.ResourceService.PasswordMinLength)
            {
                _passwordTxt.Error = "Password min length is " +
                                     ServiceInstances.ResourceService.PasswordMinLength.ToString();
                res = false;
            }

            if (_passwordTxt.Text.Length > ServiceInstances.ResourceService.PasswordMaxLength)
            {
                _passwordTxt.Error = ("Password max length is " + ServiceInstances.ResourceService.PasswordMaxLength);
                res = false;
            }

            return res;
        }

        async System.Threading.Tasks.Task LoginAsync()
        {
            if (!ValidateFields())
                return;

            var token = await ServiceInstances.AuthService.Login(_loginNameTxt.Text, _passwordTxt.Text, (err) =>
            {
                UtilUI.ErrorToast(err);
            });

            if (!string.IsNullOrEmpty(token))
            {
                var intent = new Intent(this, typeof(HomeActivity));
                intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.ClearTop | ActivityFlags.NewTask);

                StartActivity(intent);

                if (_keepLoggedinCheckBox.Checked)
                {
                    ServiceInstances.SecureStorage.StoreObject(Constants.LoggedinUser,
                        UserSession.Instance.LoggedinUser);
                    ServiceInstances.SecureStorage.Store(Constants.UserToken,
                        ServiceInstances.ResourceService.UserToken);
                }
                else
                {
                    ServiceInstances.SecureStorage.DeleteObject(Constants.LoggedinUser);
                    ServiceInstances.SecureStorage.DeleteObject(Constants.UserToken);
                }
            }
        }
    }
}