using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Webkit;
using Android.Widget;
using Asksira.WebViewSuiteLib;
using FriendLoc.Common;
using FriendLoc.Common.Models;
using FriendLoc.Common.Services;
using FriendLoc.Controls;
using FriendLoc.Droid.Adapters;
using FriendLoc.Droid.ViewModels;
using Google.Android.Material.Button;
using Google.Android.Material.Dialog;
using static Asksira.WebViewSuiteLib.WebViewSuite;

namespace FriendLoc.Droid.Dialogs
{
    public class SelectionLocationDialog : BaseDialog, IWebViewSetupInterference, IWebTrigger, TextView.IOnEditorActionListener
    {
        protected override int LayoutResId => Resource.Layout.dialog_selection_location;
        protected override string Title => "Select a Location";
        protected override string TAG => nameof(SelectionLocationDialog);
        protected override DialogTypes DialogTypes => DialogTypes.FullScreen;

        string _locationName = "";
        Coordinate _location;
        WebViewSuite _webView;
        MaterialButton _submitBtn;
        CustomEditText _nameTxt;

        public Action<Coordinate, string> OnSelected;

        public SelectionLocationDialog(Context context, Coordinate location, string locationName = "") : base(context)
        {
            _locationName = locationName;
            _location = location;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _webView = view.FindViewById<WebViewSuite>(Resource.Id.mainWebview);
            _webView.ToggleProgressbar(false);
            _webView.InterfereWebViewSetup(this);

            _nameTxt = view.FindViewById<CustomEditText>(Resource.Id.addressTxt);
            _submitBtn = view.FindViewById<MaterialButton>(Resource.Id.submitBtn);

            if (!string.IsNullOrEmpty(_locationName))
            {
                _nameTxt.Text = _locationName;
            }

            _submitBtn.Click += delegate
            {
                OnSelected?.Invoke(_location, _locationName);

                Dismiss();
            };

            _nameTxt.EditText.SetOnEditorActionListener(this);
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

            _webView.WebView.AddJavascriptInterface(new JavascriptClient(this), "Android");

            StartLoading();

            if (!string.IsNullOrEmpty(_locationName))
            {
                GeoDecoder(_locationName, (coor) =>
                {
                    StopLoading();
                    _location = coor;
                    _webView.StartLoading(Constants.MapUrl);
                });
            }
            else
            {
                GeoDecoder(_location.Latitude, _location.Longitude, (coor) =>
                {
                    StopLoading();
                    _location = coor;
                    _nameTxt.Text = _locationName;
                    _webView.StartLoading(Constants.MapUrl);
                });
            }
        }


        void GeoDecoder(IList<Address> locations, Action<Coordinate> onSelected)
        {
            if (locations == null || !locations.Any())
            {
                return;
            }

            if (locations.Count == 1)
            {
                var newLoc = locations[0];

                _locationName = newLoc.GetAddressLine(0);

                onSelected(new Coordinate()
                {
                    Latitude = newLoc.Latitude,
                    Longitude = newLoc.Longitude
                });

                return;
            }

            var items = new List<SpinnerItem>();

            foreach (var location in locations)
            {
                items.Add(new SingleTitleSpinnerItem()
                {
                    LeftImgResId = Resource.Drawable.ic_place_24,
                    MainTitle = location.GetAddressLine(0),
                    Value = new Coordinate()
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    }
                });
            }

            new MaterialAlertDialogBuilder(Context).SetAdapter(new SpinnerAdapter(items, Context), new DialogLisenter((pos) =>
            {
                _locationName = (items[pos]).MainTitle;

                onSelected((Coordinate)items[pos].Value);

            })).SetTitle("Select Location").Show();
        }


        async void GeoDecoder(double lat, double lng, Action<Coordinate> onSelected)
        {
            var coder = new Geocoder(Context);

            if (!Geocoder.IsPresent)
                return;

            var locations = await coder.GetFromLocationAsync(lat, lng, 5);

            GeoDecoder(locations, onSelected);
        }

        async void GeoDecoder(string name, Action<Coordinate> onSelected)
        {
            var coder = new Geocoder(Context);

            if (!Geocoder.IsPresent)
                return;

            var locations = await coder.GetFromLocationNameAsync(name, 5);

            GeoDecoder(locations, onSelected);
        }

        public void OnReady()
        {
            ServiceInstances.NativeTrigger.InitMap(Constants.HereMapApiKey, _location.Latitude, _location.Longitude, true);
        }

        public void LocationUpdated(Coordinate coordinate)
        {
            _location = coordinate;
        }

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            if (actionId != ImeAction.Search)
            {
                return false;
            }

            GeoDecoder(v.Text, (coor) =>
            {
                _location = coor;

                ServiceInstances.NativeTrigger.UpdateCurrentMarkerLocation(_location);

            });

            return true;
        }
    }
}
