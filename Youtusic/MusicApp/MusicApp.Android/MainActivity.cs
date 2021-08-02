﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Platform;
using MediaManager;
using GalaSoft.MvvmLight.Ioc;
using MusicApp.Services;
using MusicApp.Droid.Services;

namespace MusicApp.Droid
{
    [Activity(LaunchMode = LaunchMode.SingleTop, NoHistory = true, Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Android.Content.Intent.ActionSend },
        Categories = new[] { Android.Content.Intent.CategoryDefault },
        DataMimeTypes = new[] { "text/plain" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            SimpleIoc.Default.Register<IFileService, DroidFileService>();
            SimpleIoc.Default.Register<ISecureStorageService, DroidSecureStorageService>();

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this);
            CrossMediaManager.Current.Init(this);

            CachedImageRenderer.Init(true);
            CachedImageRenderer.InitImageViewHandler();

            XF.Material.Droid.Material.Init(this, savedInstanceState);

            var id = "";

            try
            {

                if (Intent.ClipData != null)
                {
                    string action = Intent.Action;
                    string type = Intent.Type;

                    if (Android.Content.Intent.ActionSend.Equals(action) && type.EndsWith("plain"))
                    {
                        var fileUri = Intent.ClipData;
                        if (fileUri != null)
                        {
                            var uri = fileUri.GetItemAt(0).Text;

                            if (uri.Contains("youtu"))
                                id = uri;
                        }
                    }
                }
            }
            catch
            {
                id = "";
            }

            LoadApplication(new App(id));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}