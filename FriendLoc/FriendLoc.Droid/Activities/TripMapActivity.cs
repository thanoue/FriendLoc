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
using Java.Interop;
using static Asksira.WebViewSuiteLib.WebViewSuite;

namespace FriendLoc.Droid.Activities
{
    [Activity(MainLauncher = true)]
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
            ServiceInstances.NativeTrigger.InitMap("FJqTTDyUrZA2WiGRgXWfkWEfdK98VSCgHpGpn1bkvMM", 10.877500, 106.722928);

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

        #endregion trigger
    }

    public class JavascriptClient : Java.Lang.Object
    {
        IWebTrigger _trigger;

        public JavascriptClient(IWebTrigger trigger)
        {
            _trigger = trigger;
        }

        [Android.Webkit.JavascriptInterface]
        [Export("nativeInvoke")]
        public void NativeInvoke(string command, string data)
        {
            switch (command)
            {
                case Command.documentIsReady:

                    _trigger?.OnReady();

                    break;
            }
        }

    }

    public class MyWebChromeClient : WebChromeClient
    {
        public MyWebChromeClient()
        {
        }

        [Obsolete]
        public override void OnConsoleMessage(string message, int lineNumber, string sourceID)
        {
            base.OnConsoleMessage(message, lineNumber, sourceID);

            Console.WriteLine(string.Format("LOG: {0}, lineNumber: {1}, file: {2}", message, lineNumber, sourceID));
        }
    }


}
