using System;
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
using FriendLoc.Common;
using FriendLoc.Droid.Services;

namespace FriendLoc.Droid.Activities
{
    [BroadcastReceiver(Exported = true, Enabled = true)]    
    [IntentFilter(new string[] { "RESTART" })]
    public class RestartBroadcast : BroadcastReceiver
    {
        Activity _activity;

        public RestartBroadcast()
        {

        }

        public RestartBroadcast(Activity activity)
        {
            _activity = activity;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            Log.Debug("RestartBroadcast","-------------------------------");
            var serviceIntent = new Intent(context, typeof(MyLocationService));

            //context.StopService(serviceIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                context.StartForegroundService(serviceIntent);
            }
            else
            {
                context.StartService(serviceIntent);
            }
        }
    }

    [Activity(MainLauncher = true)]
    public class TestServiceActivity : Activity
    {
        Button _startBtn;
        TextView _locationTv;
        const int REQUEST_PERMISSIONS = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_test);

            _startBtn = FindViewById<Button>(Resource.Id.startBtn);
            _locationTv = FindViewById<TextView>(Resource.Id.locationTv);

            _startBtn.Enabled = false;

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
                    .AddOnSuccessListener(new SuccessLisenter((obj) =>
                    {
                        StartService();

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
                            catch (IntentSender.SendIntentException sendEx)
                            {
                                // Ignore the error.
                            }
                        }

                    }));

            };
        }

        void StartService()
        {
            var serviceIntent = new Intent(this, typeof(MyLocationService));

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
                _startBtn.Enabled = true;
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
                _startBtn.Enabled = true;
                ServiceInstances.FileService.SetRootFolderPath(ServiceInstances.FileService.GetSdCardFolder());
            }
        }
    }
}
