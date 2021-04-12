using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Webkit;
using AndroidX.AppCompat.App;
using Asksira.WebViewSuiteLib;
using FriendLoc.Common;
using FriendLoc.Common.Models;
using FriendLoc.Common.Services;
using FriendLoc.Model;
using Java.Interop;
using Newtonsoft.Json;
using static Asksira.WebViewSuiteLib.WebViewSuite;

namespace FriendLoc.Droid.Activities
{
    [Activity(MainLauncher = false)]
    public class TripMapActivity : AppCompatActivity, IWebViewSetupInterference, IWebTrigger
    {
        WebViewSuite _webView; JavascriptClient _javascriptClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_trip_map);

            _webView = FindViewById<WebViewSuite>(Resource.Id.mainWebview);
            _webView.ToggleProgressbar(false);
            _webView.InterfereWebViewSetup(this);

#if DEBUG
            WebView.SetWebContentsDebuggingEnabled(true);
#endif
        }

        public void InterfereWebViewSetup(WebView webView)
        {
            WebSettings settings = webView.Settings;

            ServiceInstances.NativeTrigger.Init(webView);

            settings.JavaScriptEnabled = true;
            settings.SetEnableSmoothTransition(true);
            settings.DomStorageEnabled = false;
            settings.SetSupportZoom(true);
            settings.BuiltInZoomControls = true;
            settings.MixedContentMode = MixedContentHandling.AlwaysAllow;
            settings.CacheMode = CacheModes.NoCache;

            webView.ClearCache(true);

            webView.SetWebChromeClient(new MyWebChromeClient());

            _javascriptClient = new JavascriptClient(this);

            _webView.WebView.AddJavascriptInterface(_javascriptClient, "Android");

            _webView.StartLoading(Constants.MapUrl);
        }

        #region trigger

        public void OnReady()
        {
            ServiceInstances.NativeTrigger.InitMap(Constants.HereMapApiKey, 10.877500, 106.722928);

            (new Handler()).PostDelayed(() =>
            {
                var users = new List<MapUserModel>()
                {
                    new MapUserModel()
                    {
                         AvtUrl = "https://cdn3.iconfinder.com/data/icons/tourism/eiffel200.png",
                         FullName = "Kha",
                         Id = "123",
                         Lat = 10.877500,
                         Lng =  106.722928
                    },
                    new MapUserModel()
                    {
                         AvtUrl = "https://cdn3.iconfinder.com/data/icons/tourism/eiffel200.png",
                         FullName = "Kha",
                         Id = "1234",
                         Lat = 10.877500,
                         Lng =  106.726928
                    },
                    new MapUserModel()
                    {
                         AvtUrl = "https://cdn3.iconfinder.com/data/icons/tourism/eiffel200.png",
                         FullName = "Kha",
                         Id = "1253",
                         Lat = 10.877500,
                         Lng =  106.723928
                    },
                    new MapUserModel()
                    {
                         AvtUrl = "https://cdn3.iconfinder.com/data/icons/tourism/eiffel200.png",
                         FullName = "Kha",
                         Id = "1237",
                         Lat = 10.877500,
                         Lng =  106.782928
                    },
                    new MapUserModel()
                    {
                         AvtUrl = "https://cdn3.iconfinder.com/data/icons/tourism/eiffel200.png",
                         FullName = "Kha",
                         Id = "1237r5",
                         Lat = 10.877120,
                         Lng =  106.782928
                    },
                    new MapUserModel()
                    {
                         AvtUrl = "https://cdn3.iconfinder.com/data/icons/tourism/eiffel200.png",
                         FullName = "Kdha",
                         Id = "1f23s7r5",
                         Lat = 10.877220,
                         Lng =  106.782928
                    },
                };

                ServiceInstances.NativeTrigger.AddUsers(users);
            }, 2000);

        }

        public void LocationUpdated(Coordinate coordinate)
        {

        }

        #endregion trigger
    }
}
