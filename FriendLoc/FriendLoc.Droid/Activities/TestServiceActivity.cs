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
using Firebase.Auth;
using Firebase.Database;
using FriendLoc.Common;
using FriendLoc.Common.Models;
using FriendLoc.Droid.Services;

namespace FriendLoc.Droid.Activities
{
    [Activity(MainLauncher = true)]
    public class TestServiceActivity : Activity
    {
        const int REQUEST_PERMISSIONS = 1;

        Button _startBtn, _loginBtn, _getRecordsBtn;
        TextView _locationTv;
        string _userId, _tripId = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_test);

            _startBtn = FindViewById<Button>(Resource.Id.startBtn);
            _locationTv = FindViewById<TextView>(Resource.Id.locationTv);
            _loginBtn = FindViewById<Button>(Resource.Id.loginBtn);
            _getRecordsBtn = FindViewById<Button>(Resource.Id.getRecordsBtn);

            _startBtn.Enabled = false;
            _loginBtn.Enabled = false;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                CheckPermission(Android.Manifest.Permission.AccessBackgroundLocation,
                                 Android.Manifest.Permission.AccessFineLocation,
                                 Android.Manifest.Permission.AccessCoarseLocation,
                                 Android.Manifest.Permission.ControlLocationUpdates,
                                 Android.Manifest.Permission.Camera,
                                 Android.Manifest.Permission.ReadExternalStorage,
                                 Android.Manifest.Permission.WriteExternalStorage);
            }
            else
            {
                CheckPermission(Android.Manifest.Permission.AccessFineLocation,
                        Android.Manifest.Permission.AccessCoarseLocation,
                        Android.Manifest.Permission.ControlLocationUpdates,
                        Android.Manifest.Permission.Camera,
                        Android.Manifest.Permission.ReadExternalStorage,
                        Android.Manifest.Permission.WriteExternalStorage);
            }

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

                        var adding = await ServiceInstances.TripRepository.InsertAsync(new Common.Models.Trip()
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
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCxjza0PW9fg6y4tPlljkP-iBSwOC0XY6g"));

                var login = await authProvider.SignInWithEmailAndPasswordAsync("kha@gmail.com", "Hello_2020");

                if (login != null)
                {
                    _userId = login.User.LocalId;

                    var firebase = new FirebaseClient(
                    "https://friendloc-98ed3-default-rtdb.firebaseio.com/",
                    new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(login.FirebaseToken)
                    });

                    ServiceInstances.TripRepository.Init(firebase);
                    ServiceInstances.UserRepository.Init(firebase);

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

        bool IsGranted(params string[] pers)
        {
            foreach (var per in pers)
            {
                if (ActivityCompat.CheckSelfPermission(this, per) != Permission.Granted)
                {
                    return false;
                }
            }

            return true;
        }

        void CheckPermission(params string[] pers)
        {
            if (!IsGranted(pers))
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, pers[0]))
                {
                    ActivityCompat.RequestPermissions(this, pers,
                      REQUEST_PERMISSIONS);
                }
                else
                {
                    ActivityCompat.RequestPermissions(this, pers,
                      REQUEST_PERMISSIONS);
                }
            }
            else
            {
                _loginBtn.Enabled = true;
                ServiceInstances.FileService.SetRootFolderPath(ServiceInstances.FileService.GetSdCardFolder());
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode != REQUEST_PERMISSIONS)
                return;

            if (grantResults.Length > 0 && grantResults[0] == Permission.Denied)
            {
                Toast.MakeText(ApplicationContext, "Please allow the permission", ToastLength.Long).Show();
            }
            else
            {
                _loginBtn.Enabled = true;
                ServiceInstances.FileService.SetRootFolderPath(ServiceInstances.FileService.GetSdCardFolder());
            }
        }
    }
}
