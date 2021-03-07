
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

namespace FriendLoc.Droid.Activities
{
    [Activity]
    public class SignUpActivity : BaseActivity
    {
        protected override bool IsConfirmBeforeBack => true;
        protected override int LayoutResId => Resource.Layout.activity_signup;
        protected override string HeaderTitle => "Sign up";

        ShapeableImageView _avtImg;
        TextInputLayout _countryCodeDropDown;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _avtImg = FindViewById<ShapeableImageView>(Resource.Id.avtImg);
            _countryCodeDropDown = FindViewById<TextInputLayout>(Resource.Id.countryCodeDropDown);

            _avtImg.SetImageResource(Resource.Drawable.ic_account_24);
            _avtImg.SetScaleType(ImageView.ScaleType.FitXy);

            FindViewById<MaterialButton>(Resource.Id.submitBtn).Click += delegate
            {
                OnSaveRequest();
            };

            FindViewById<ExtendedFloatingActionButton>(Resource.Id.updatImgBtn).Click += delegate
            {
                CheckPermision(() =>
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
            };

            var countryCodes = new Java.Lang.Object[]
            {
               "+84","+62","+78","+100"
            };

            var adapter = new ArrayAdapter(this, Resource.Layout.list_item, countryCodes);

            (_countryCodeDropDown.EditText as AutoCompleteTextView).Adapter = (adapter);

        }

        protected override void OnSaveRequest()
        {

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
                    _avtImg.SetScaleType(ImageView.ScaleType.CenterCrop);
                    Glide.With(this).Load(Android.Net.Uri.FromFile(new Java.IO.File(urls[0].Path))).Into(_avtImg);
                }
            }
        }
    }
}
