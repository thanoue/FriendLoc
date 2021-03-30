using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase.Auth;
using Firebase.Database;
using FriendLoc.Common;
using FriendLoc.Droid.Services;
using FriendLoc.Entity;

namespace FriendLoc.Droid.Activities
{
    [Activity(MainLauncher = false)]
    public class TestServiceActivity : BaseActivity
    {

        Button _startBtn, _loginBtn, _getRecordsBtn;
        TextView _locationTv;
        string _userId, _tripId = "";

        protected override int LayoutResId => Resource.Layout.activity_test;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            _startBtn = FindViewById<Button>(Resource.Id.startBtn);
            _locationTv = FindViewById<TextView>(Resource.Id.locationTv);
            _loginBtn = FindViewById<Button>(Resource.Id.loginBtn);
            _getRecordsBtn = FindViewById<Button>(Resource.Id.getRecordsBtn);

            _startBtn.Enabled = false;
            _loginBtn.Enabled = false;

            _startBtn.Click += delegate
            {
                LocationRequest locationRequest = LocationRequest.Create();
                locationRequest.SetInterval(10000);
                locationRequest.SetFastestInterval(5000);
                locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);

                LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
                builder.AddLocationRequest(locationRequest);

                SettingsClient client = LocationServices.GetSettingsClient(this);
                var task = client.CheckLocationSettings(builder.Build())
                    .AddOnSuccessListener(new SuccessLisenter(async (obj) =>
                    {
                        var userIds = new Dictionary<string, string>();

                        userIds.Add(_userId, _userId);

                        var adding = await ServiceInstances.TripRepository.InsertAsync(new Trip()
                        {
                            Description = "Test trips",
                            UserIds = userIds
                        });

                        if (adding != null)
                        {
                            _tripId = adding.Id;
                            StartService(_tripId, _userId);
                        }

                    })).AddOnFailureListener(new FailureLisenter((ex) =>
                    {
                        if (ex is ResolvableApiException)
                        {
                            try
                            {
                                ResolvableApiException resolvable = (ResolvableApiException)ex;
                                resolvable.StartResolutionForResult(this,
                                        100);
                            }
                            catch (IntentSender.SendIntentException e2x)
                            {
                                // Ignore the error.
                            }
                        }

                    }));

            };

            _loginBtn.Click += async delegate
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));

                var login = await authProvider.SignInWithEmailAndPasswordAsync("kha@gmail.com", "Hello_2020");

                if (login != null)
                {
                    _userId = login.User.LocalId;

                    var firebase = new FirebaseClient(
                    Constants.FirebaseDbPath,
                    new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(login.FirebaseToken)
                    });


                    _startBtn.Enabled = true;
                }

            };

            _getRecordsBtn.Click += async delegate
            {
                var res = await ServiceInstances.TripRepository.GetById(_tripId);

                Console.WriteLine(res.Locations.Values.ToList()[0].CreatedTime);
            };
        }

        void StartService(string tripId, string userId)
        {
            var serviceIntent = new Intent(this, typeof(MyLocationService));
            serviceIntent.PutExtra(Constants.TripId, tripId);
            serviceIntent.PutExtra(Constants.UserId, userId);

            StopService(serviceIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                StartForegroundService(serviceIntent);
            }
            else
            {
                StartService(serviceIntent);
            }
        }

    }
}
