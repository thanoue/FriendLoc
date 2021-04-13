using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Gms.Tasks;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using FriendLoc.Common;
using FriendLoc.Entity;
using Location = Android.Locations.Location;

namespace FriendLoc.Droid.Services
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
            Log.Debug("RestartBroadcast", "-------------------------------");
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

    [Service(ForegroundServiceType = ForegroundService.TypeLocation | ForegroundService.TypeDataSync,Name = "FriendLoc.Droid.Services.MyLocationService")]
    public class MyLocationService : Service
    {
        const string NOTIFY_CHANEL_ID = "FriendLoc";
        const string TAG = "MyLocationService";

        private FusedLocationProviderClient _fusedLocationClient;
        private CusLocationCallback _callback;
        string _tripid;
        string _userId;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);

            _tripid = intent.GetStringExtra(Constants.TripId);
            _userId = intent.GetStringExtra(Constants.UserId);

            return StartCommandResult.NotSticky;
        }   

        public override void OnTaskRemoved(Intent rootIntent)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                Intent broadcastIntent = new Intent();
                broadcastIntent.SetAction("RESTART");
                broadcastIntent.SetClass(this, typeof(RestartBroadcast));

                this.SendBroadcast(broadcastIntent);
             }

            base.OnTaskRemoved(rootIntent);
        }

        public override void OnCreate()
        {
            base.OnCreate();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);

                var notifyChannel = new NotificationChannel(
                                                       NOTIFY_CHANEL_ID,
                                                       "FriendLoc",
                                                       Android.App.NotificationImportance.Max);
                notificationManager.CreateNotificationChannel(notifyChannel);

                Notification notification = new NotificationCompat.Builder(this, NOTIFY_CHANEL_ID)
                        .SetContentTitle("")
                        .SetPriority(NotificationCompat.PriorityMax)
                        .SetContentText("Your location is updating on background").Build();

                StartForeground(1, notification);
            }

            LocationRequest locationRequest = LocationRequest.Create();
            locationRequest.SetInterval(10000);
            locationRequest.SetFastestInterval(5000);
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);

            _fusedLocationClient = LocationServices.GetFusedLocationProviderClient(this);

            _callback = new CusLocationCallback((location)=> {

                // ServiceInstances.TripRepository.AddLocation(_tripid, new TripLocation()
                // {
                //     Latitude = location.Latitude,
                //     Longitude = location.Longitude,
                //      TripId = _tripid,
                //       UserId = _userId
                // });

            });

            _fusedLocationClient.RequestLocationUpdates(locationRequest, _callback, Looper.MainLooper);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            _fusedLocationClient.RemoveLocationUpdates(_callback);
        }
    }

    public class CusLocationCallback : LocationCallbackBase
    {
        Action<Location> _onLocationChanged;
        public CusLocationCallback(Action<Location> onLocationChanged)
        {
            _onLocationChanged = onLocationChanged;
        }

        Location _lastestLocation;
        public override void OnLocationResult(LocationResult locationResult)
        {
            if (locationResult == null)
            {
                return;
            }

            // foreach (Location location in locationResult.Locations)
            // {
            //     if (_lastestLocation == null)
            //     {
            //         _onLocationChanged?.Invoke(location);
            //         _lastestLocation = location;
            //     }
            //
            //     var distance = _lastestLocation.DistanceTo(location);
            //
            //     if (distance <= 20)
            //     {
            //         continue;
            //     }
            //
            //     _lastestLocation.Set(location);
            //
            //     _onLocationChanged?.Invoke(location);
            //
            //     //Console.WriteLine(location.ToString());
            //
            //     //var logger = ServiceInstances.LoggerService;
            //
            //     //if (logger == null)
            //     //{
            //     //    var fileService = new DroidFileService(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Constants.APP_NAME));
            //
            //     //    logger = new LoggerService(fileService);
            //     //}
            //
            //     //logger.Info(string.Format("Latitude : {0}, \n Longitude: {1}, \n Altitude: {2}", location.Latitude.ToString(), location.Longitude.ToString(), location.Altitude));
            //
            // }
        }
    }


    public class SuccessLisenter : Java.Lang.Object, IOnSuccessListener
    {
        Action<Java.Lang.Object> _callback;
        public SuccessLisenter(Action<Java.Lang.Object> callback)
        {
            _callback = callback;
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            _callback?.Invoke(result);
        }
    }

    public class FailureLisenter : Java.Lang.Object, IOnFailureListener
    {
        Action<Java.Lang.Exception> _callback;
        public FailureLisenter(Action<Java.Lang.Exception> callback)
        {
            _callback = callback;
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            _callback?.Invoke(e);
        }
    }   


}
