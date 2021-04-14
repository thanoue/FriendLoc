using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Extensions;
using Android.Gms.Location;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using BumpTech.GlideLib;
using FriendLoc.Common;
using FriendLoc.Common.Models;
using FriendLoc.Controls;
using FriendLoc.Droid.Activities;
using FriendLoc.Droid.Services;
using FriendLoc.Entities;
using FriendLoc.Entity;
using Google.Android.Material.Button;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.ImageView;
using Plugin.CurrentActivity;

namespace FriendLoc.Droid.Dialogs
{
    public class AddMilestoneDialog : BaseDialog, IImgSelectionObj
    {
        protected override int LayoutResId => Resource.Layout.dialog_addmilestone;
        protected override string Title => "Add new Milestone";
        protected override DialogTypes DialogTypes => DialogTypes.Popup;

        protected override string TAG => nameof(AddMilestoneDialog);

        MaterialButton _selectLocationBtn;
        CustomEditText _addressTxt, _nameTxt;
        Coordinate _location;
        CusLocationCallback _callback;
        FusedLocationProviderClient _locationProviderClient;
        Milestone _newMilestone;
        ShapeableImageView _avtImg;
        ExtendedFloatingActionButton _selectAvtBtn;
        MaterialButton _submitBtn;
        string _imgUrl;
        public Action<Coordinate, string> OnSelected;

        public AddMilestoneDialog(Context context) : base(context)
        {
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _selectLocationBtn = view.FindViewById<MaterialButton>(Resource.Id.selectPositionBtn);
            _addressTxt = view.FindViewById<CustomEditText>(Resource.Id.addressTxt);
            _avtImg = view.FindViewById<ShapeableImageView>(Resource.Id.avtImg);
            _selectAvtBtn = view.FindViewById<ExtendedFloatingActionButton>(Resource.Id.updatImgBtn);
            _submitBtn = view.FindViewById<MaterialButton>(Resource.Id.submitBtn);
            _nameTxt = view.FindViewById<CustomEditText>(Resource.Id.nameTxt);

            _avtImg.Click += delegate { SelectAvt(); };

            _selectAvtBtn.Click += delegate { SelectAvt(); };

            _selectLocationBtn.Click += delegate
            {
                var selectLoc = new SelectionLocationDialog(Context, _location, _addressTxt.Text);

                selectLoc.OnSelected = (coor, locName) =>
                {
                    _addressTxt.Text = locName;
                    _location = new Coordinate()
                    {
                        Longitude = coor.Longitude,
                        Latitude = coor.Latitude
                    };
                };

                selectLoc.ShowDialog();
            };

            _submitBtn.Click += delegate { SubmitAsync(); };

            GetCurrentLocation();
        }

        async void GetCurrentLocation()
        {
            _locationProviderClient = LocationServices.GetFusedLocationProviderClient(CurrentActivity);

            LocationRequest locationRequest = LocationRequest.Create();
            locationRequest.SetInterval(5000);
            locationRequest.SetFastestInterval(5000);
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);

            LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
            builder.AddLocationRequest(locationRequest);

            SettingsClient client = LocationServices.GetSettingsClient(CurrentActivity);

            var task = await client.CheckLocationSettings(builder.Build())
                .AddOnFailureListener(new FailureLisenter((ex) =>
                {
                    if (ex is ResolvableApiException)
                    {
                        try
                        {
                            ResolvableApiException resolvable = (ResolvableApiException) ex;
                            resolvable.StartResolutionForResult(CrossCurrentActivity.Current.Activity,
                                100);

                            Dismiss();
                        }
                        catch (IntentSender.SendIntentException e2x)
                        {
                            // Ignore the error.
                        }
                    }
                }));

            UtilUI.StartLoading();

            if (task != null)
            {
                Task.Run(() =>
                {
                    _callback = new CusLocationCallback(OnLocationGot);

                    _locationProviderClient.RequestLocationUpdates(locationRequest, _callback, Looper.MainLooper);
                });
            }
        }

        void OnLocationGot(Android.Locations.Location location)
        {
            _locationProviderClient.RemoveLocationUpdates(_callback);

            UtilUI.StopLoading();

            _location = new Coordinate()
            {
                Longitude = location.Longitude,
                Latitude = location.Latitude
            };
        }

        private async void SubmitAsync()
        {
            if (!string.IsNullOrEmpty(_imgUrl))
            {
                var path = await ServiceInstances.AuthService.PushImageToServer(_imgUrl, (process) => { },
                    Constants.MileStoneStorageFolderName);

                if (!string.IsNullOrEmpty(path))
                {
                    _imgUrl = path;
                    SaveChanges();
                }
                else
                {
                    SaveChanges();
                }
            }
            else
            {
                SaveChanges();
            }
        }

        async void SaveChanges()
        {
            var milestone = new Milestone()
            {
                Address = _addressTxt.Text,
                Name = _nameTxt.Text,
                DisplayImgUrl = _imgUrl,
                Latitude = _location.Latitude,
                Longitude = _location.Longitude
            };

            await ServiceInstances.UserMilestoneRepository.AddMileStone(UserSession.Instance.LoggedinUser.Id,
                milestone);

            OnSelected?.Invoke(new Coordinate()
            {
                Latitude = _location.Latitude,
                Longitude = _location.Longitude
            }, milestone.Name);

            this.Dismiss();
        }

        void SelectAvt()
        {
            CurrentActivity.SetImgSelectionListner(this);
            CurrentActivity.SelectImageFromGallery();
        }

        public void OnImgSelected(string path, Activity activity)
        {
            using (var bitmap = BitmapFactory.DecodeFile(path))
            {
                _imgUrl = path;
                _avtImg.SetScaleType(ImageView.ScaleType.CenterCrop);
                Glide.With(activity).Load(bitmap).Into(_avtImg);
            }
        }
    }
}