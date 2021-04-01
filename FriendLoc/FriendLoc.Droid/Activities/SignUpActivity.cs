
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
using Com.Nguyenhoanglam.Imagepicker.UI.Imagepicker;
using Com.Nguyenhoanglam.Imagepicker.Model;
using Google.Android.Material.AppBar;
using Google.Android.Material.ImageView;
using FriendLoc.Common;
using Android;
using BumpTech.GlideLib;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.TextField;
using Google.Android.Material.Button;
using Google.Android.Material.Dialog;
using System.IO;
using Firebase.Auth;
using System.Threading.Tasks;
using Android.Graphics;
using FriendLoc.Controls;
using User = FriendLoc.Entity.User;
using FriendLoc.Entity;
using FriendLoc.Model;
using FriendLoc.Droid.Fragments;

namespace FriendLoc.Droid.Activities
{
    [Activity]
    public class SignUpActivity : BaseActivity
    {
        protected override bool IsAskBeforeDismiss => true;
        protected override int LayoutResId => Resource.Layout.activity_signup;
        protected override string HeaderTitle => "Sign up";

        ShapeableImageView _avtImg;
        MaterialButton _submitBtn;
        string _avtPath = "";
        CustomEditText _countryCodeDropDown, _loginNameTxt, _passwordTxt, _retypePasswordtxt, _fullNameTxt, _phoneNumbtxt;
        RadioButton _maleGenderRadio, _feMaleGenderRadio;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _avtImg = FindViewById<ShapeableImageView>(Resource.Id.avtImg);
            _countryCodeDropDown = FindViewById<CustomEditText>(Resource.Id.countryCodeDropDown);
            _submitBtn = FindViewById<MaterialButton>(Resource.Id.submitBtn);
            _loginNameTxt = FindViewById<CustomEditText>(Resource.Id.userNameTxt);
            _passwordTxt = FindViewById<CustomEditText>(Resource.Id.passwordTxt);
            _retypePasswordtxt = FindViewById<CustomEditText>(Resource.Id.retypePasswordTxt);
            _fullNameTxt = FindViewById<CustomEditText>(Resource.Id.userFullNameTxt);
            _phoneNumbtxt = FindViewById<CustomEditText>(Resource.Id.phoneNumberTxt);
            _maleGenderRadio = FindViewById<RadioButton>(Resource.Id.maleRadio);
            _feMaleGenderRadio = FindViewById<RadioButton>(Resource.Id.feMaleRadio);

            _maleGenderRadio.Checked = true;

            _avtImg.SetImageResource(Resource.Drawable.ic_account_24);
            _avtImg.SetScaleType(ImageView.ScaleType.FitXy);

            FindViewById<ExtendedFloatingActionButton>(Resource.Id.updatImgBtn).Click += delegate
            {
                SelectAvt();
            };

            _avtImg.Click += delegate
            {
                //this.LoadFragment(new AddTripFragment());
                SelectAvt();
            };

            var countryCodes = new Java.Lang.Object[]
            {
               "+84","+62","+78","+100"
            };

            var adapter = new ArrayAdapter(this, Resource.Layout.list_item, countryCodes);

            (_countryCodeDropDown.EditText as AutoCompleteTextView).Adapter = (adapter);

            _submitBtn.Click += delegate
            {
                OnSaveRequest();
            };
        }

        void SelectAvt()
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

            if (!_passwordTxt.Text.Equals(_retypePasswordtxt.Text))
            {
                _retypePasswordtxt.Error = "Retype Password is not match";
                res = false;
            }

            if (string.IsNullOrEmpty(_fullNameTxt.Text))
            {
                _fullNameTxt.Error = "Full Name is required";
                res = false;
            }
            return res;
        }

        protected override async void OnSaveRequest()
        {
            if (!ValidateFields())
                return;

            StartLoading("Creating new Account...");

            var userModel = new SignUpModel()
            {
                Gender = _maleGenderRadio.Checked ? Gender.Male : _feMaleGenderRadio.Checked ? Gender.Female : Gender.Others,
                AvtImgPath = _avtPath,
                CountryCode = _countryCodeDropDown.Text,
                FullName = _fullNameTxt.Text,
                LoginName = _loginNameTxt.Text,
                Password = _passwordTxt.Text,
                PhoneNumber = _phoneNumbtxt.Text
            };

            ServiceInstances.AuthService.SignUp(userModel, (err) =>
            {
                RunOnUiThread(() =>
                {
                    ErrorToast(err);
                    StopLoading();
                });

            }, () =>
            {
                StopLoading();
                Finish();
            });
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
                    _avtPath = urls[0].Path;
                    using (var bitmap = BitmapFactory.DecodeFile(_avtPath))
                    {
                        _avtImg.SetScaleType(ImageView.ScaleType.CenterCrop);
                        Glide.With(this).Load(bitmap).Into(_avtImg);
                    }
                }
            }
        }
    }
}
